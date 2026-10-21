using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Brands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanArchCQRSandMediator.Application.Brands.Queries.GetBrandById
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetBrandByIdQueryHandler(IApplicationDbContext context, IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<BrandDto> Handle(
            GetBrandByIdQuery request,
            CancellationToken cancellationToken)
        {
            var brand = await _context.Brands
                .AsNoTracking()
                .Where(b => b.Id == request.BrandId)
                .Select(b => new BrandDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    Code = b.Code,
                    TenantId = b.TenantId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (brand == null)
                throw new NotFoundException(_localizer["Organization_Not_Found"]);

            return brand;
        }
    }
}
