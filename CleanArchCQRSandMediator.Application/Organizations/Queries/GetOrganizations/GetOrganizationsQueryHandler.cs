using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Common.Models;
using CleanArchCQRSandMediator.Application.Dtos.Organizations;
using CleanArchCQRSandMediator.Domain.Entities.Business;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace CleanArchCQRSandMediator.Application.Organizations.Queries.GetOrganizations
{
    public class GetOrganizationsQueryHandler : IRequestHandler<GetOrganizationsQuery, PagedResult<OrganizationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserAuthorizationService _userAuth;

        public GetOrganizationsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IUserAuthorizationService userAuth)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userAuth = userAuth;
        }

        public async Task<PagedResult<OrganizationDto>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;

            // 1. Base query (AsNoTracking for read-only).
            var query = _context.Tenants.AsNoTracking().AsQueryable();

            // 2. Restriction by user (except Super Administrator).
            var isSuperAdmin = await _userAuth.IsSuperAdminAsync();
            if (!isSuperAdmin)
            {
                var userId = _currentUserService.GetUserId();

                // The user only sees the tenants to which they belong.
                query = query.Where(t =>
                    t.ApplicationUserTenants.Any(aut => aut.ApplicationUserId == userId));
            }

            // 3. Filters
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToLowerInvariant();
                query = query.Where(o =>
                    o.Name.ToLower().Contains(search) ||
                    o.Description.ToLower().Contains(search) ||
                    o.Slug.ToLower().Contains(search));
            }

            if (filter.IsActive.HasValue)
                query = query.Where(o => o.IsActive == filter.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(filter.Slug))
            {
                var slug = filter.Slug.Trim().ToLowerInvariant();
                query = query.Where(o => o.Slug == slug);
            }

            // 4. Total records (before pagination)
            var totalCount = await query.CountAsync(cancellationToken);

            // 5. Ordering
            query = ApplySorting(query, filter.SortBy, filter.SortOrder);

            // 6. Pagination + projection to DTO
            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(o => new OrganizationDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    Slug = o.Slug,
                    IsActive = o.IsActive
                })
                .ToListAsync(cancellationToken);

            // 7. Paginated result
            return new PagedResult<OrganizationDto>
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
        private static IQueryable<Tenant> ApplySorting(
            IQueryable<Tenant> query,
            string sortBy,
            string sortOrder)
        {
            var isDescending = sortOrder?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;

            // Whitelist of fields allowed for sorting
            return sortBy?.ToLowerInvariant() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(o => o.Name)
                    : query.OrderBy(o => o.Name),

                "slug" => isDescending
                    ? query.OrderByDescending(o => o.Slug)
                    : query.OrderBy(o => o.Slug),

                /* "createdat" => isDescending
                    ? query.OrderByDescending(o => o.CreatedAt)
                    : query.OrderBy(o => o.CreatedAt), */

                "isactive" => isDescending
                    ? query.OrderByDescending(o => o.IsActive)
                    : query.OrderBy(o => o.IsActive),

                // By default, sort by Name in ascending order.
                _ => query.OrderBy(o => o.Name)
            };
        }
    }
}
