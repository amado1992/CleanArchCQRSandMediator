using CleanArchCQRSandMediator.Application.Dtos.Brands;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Brands.Queries.GetBrandById
{
    public record GetBrandByIdQuery : IRequest<BrandDto>
    {
        public int BrandId { get; set; }
    }
}
