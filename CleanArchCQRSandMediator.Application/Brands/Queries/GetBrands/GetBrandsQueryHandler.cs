using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Common.Models;
using CleanArchCQRSandMediator.Application.Dtos.Brands;
using CleanArchCQRSandMediator.Domain.Entities.Business.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.Application.Brands.Queries.GetBrands
{
    public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, PagedResult<BrandListItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserAuthorizationService _userAuth;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetBrandsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IUserAuthorizationService userAuth, IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userAuth = userAuth;
            _localizer = localizer;
        }

        public async Task<PagedResult<BrandListItemDto>> Handle(
            GetBrandsQuery request,
            CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            // 1. Base query (AsNoTracking for read-only)
            var query = _context.Brands.AsNoTracking().AsQueryable();

            var isSuperAdmin = await _userAuth.IsSuperAdminAsync(cancellationToken);

            // 2. Filter by TenantId (Organization)
            if (filter.TenantId.HasValue)
            {
                await _userAuth.EnsureCanAccessTenantAsync(filter.TenantId.Value, cancellationToken);
                query = query.Where(b => b.TenantId == filter.TenantId.Value);
            }
            else if (!isSuperAdmin)
            {
                var tenantId = await _userAuth.GetDefaultTenantIdAsync(cancellationToken);

                if (!tenantId.HasValue)
                    throw new UnauthorizedException(_localizer["UserNotHaveAssignedOrganization"]);

                query = query.Where(b => b.TenantId == tenantId);
            }


            // 2. Filters

            // Text search (Name, Description)
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToLowerInvariant();
                query = query.Where(b =>
                    b.Name.ToLower().Contains(search) ||
                    b.Description.ToLower().Contains(search));
            }

            // 3. Total records (before pagination)
            var totalCount = await query.CountAsync(cancellationToken);

            // 4. Ordering (whitelist to prevent field injection)
            query = ApplySorting(query, filter.SortBy, filter.SortOrder);

            // 5. Pagination + projection to DTO
            //    OrganizationName = b.Tenant.Name (Tenant = Organization)
            //    EF Core automatically translates it into an INNER JOIN.
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(b => new BrandListItemDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    OrganizationName = b.Tenant.Name  // JOIN with tenants
                })
                .ToListAsync(cancellationToken);

            // 6. Paginated result
            return new PagedResult<BrandListItemDto>
            {
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Apply the sorting according to the parameters, using a whitelist.
        /// </summary>
        private static IQueryable<Brand> ApplySorting(
            IQueryable<Brand> query,
            string sortBy,
            string sortOrder)
        {
            var isDescending = sortOrder?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;

            // Whitelist of fields allowed for sorting
            return sortBy?.ToLowerInvariant() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(b => b.Name)
                    : query.OrderBy(b => b.Name),

                "description" => isDescending
                    ? query.OrderByDescending(b => b.Description)
                    : query.OrderBy(b => b.Description),

                "organization" => isDescending
                    ? query.OrderByDescending(b => b.Tenant.Name)
                    : query.OrderBy(b => b.Tenant.Name),

                "createdat" => isDescending
                    ? query.OrderByDescending(b => b.CreatedAt)
                    : query.OrderBy(b => b.CreatedAt),

                // By default, sort by Name in ascending order.
                _ => query.OrderBy(b => b.Name)
            };
        }
    }
}
