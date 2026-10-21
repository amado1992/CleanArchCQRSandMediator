using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Business.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.Application.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DeleteBrandCommandHandler(IApplicationDbContext context, IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(
            DeleteBrandCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Search for the brand by ID
            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.Id == request.BrandId, cancellationToken);

            if (brand == null)
                throw new NotFoundException(_localizer["Brand_Not_Found"]);

            // 2. Check if there are any associated products (optional but recommended)
            /* var hasProducts = await _context.Products
                .AnyAsync(p => p.BrandId == request.BrandId, cancellationToken);

            if (hasProducts)
                throw new ConflictException(_localizer["Brand_With_Associated_Products"]); */

            // 3. Soft delete: mark as inactive
            // brand.IsActive = false;

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}   
