using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Business.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;


namespace CleanArchCQRSandMediator.Application.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<CreateBrandCommandHandler> _localizer;

        public CreateBrandCommandHandler(IApplicationDbContext context, IStringLocalizer<CreateBrandCommandHandler> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<int> Handle(
            CreateBrandCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Verify that the tenant exists and is active
            var tenantExists = await _context.Tenants
                .AnyAsync(t => t.Id == request.TenantId && t.IsActive, cancellationToken);

            if (!tenantExists)
                throw new NotFoundException(_localizer["Organization_Not_Found"]);

            // 2. Normalize the code (uppercase, no spaces)
            var normalizedName = request.Name.Trim();
            var normalizedCode = request.Code.Trim().ToUpperInvariant();

            /* // 3. Verify that the code is unique within the tenant
            var codeExists = await _context.Brands
                .AnyAsync(b => b.Code == normalizedCode && b.TenantId == request.TenantId,
                           cancellationToken);

            if (codeExists)
                throw new ConflictException(_localizer["Brand_With_Code_Already_Exists", normalizedCode]);*/

            // 4. Verify that the name is unique within the tenant (case-insensitive)
            var nameExists = await _context.Brands
                .AnyAsync(b => b.TenantId == request.TenantId
                            && b.Name.ToLower() == normalizedName.ToLower(),
                          cancellationToken);

            if (nameExists)
                throw new ConflictException(_localizer["Brand_With_Name_Already_Exists", normalizedName]);

            // 5. Create the entity
            var brand = new Brand
            {
                Name = normalizedName,
                Description = request.Description?.Trim() ?? string.Empty,
                Code = normalizedCode,
                TenantId = request.TenantId
            };

            // 6. Save
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync(cancellationToken);

            return brand.Id;
        }
    }
}
