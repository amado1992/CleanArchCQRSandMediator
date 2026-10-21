using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.Application.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateBrandCommandHandler(IApplicationDbContext context, IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(
            UpdateBrandCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Normalize the code (uppercase, no spaces)
            var normalizedCode = request.Code.Trim().ToUpperInvariant();
            var normalizedName = request.Name.Trim();

            // 2. Search for the brand by ID.
            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

            if (brand == null)
                throw new NotFoundException(_localizer["Brand_Not_Found"]);

            // 4. Verify that the new name is not already in use by another brand within the same tenant (case-insensitive)
            var nameExists = await _context.Brands
                .AnyAsync(b => b.TenantId == brand.TenantId
                            && b.Name.ToLower() == normalizedName.ToLower()
                            && b.Id != request.Id,
                          cancellationToken);

            if (nameExists)
                throw new ConflictException(
                    _localizer["Brand_With_Name_Already_Exists", normalizedName]);

            // 4. Update allowed properties
            brand.Name = normalizedName;
            brand.Description = request.Description?.Trim() ?? string.Empty;
            brand.Code = normalizedCode;
            // TenantId is not updated (the brand remains in its original tenant)

            // 5. Save (UpdatedAt is handled automatically in SaveChangesAsync)
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
