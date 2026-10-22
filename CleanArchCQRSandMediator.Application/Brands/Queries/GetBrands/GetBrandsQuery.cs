using CleanArchCQRSandMediator.Application.Common.Models;
using CleanArchCQRSandMediator.Application.Dtos.Brands;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Brands.Queries.GetBrands
{
    public class GetBrandsQuery : IRequest<PagedResult<BrandListItemDto>>
    {
        public BrandFilterDto Filter { get; set; } = new BrandFilterDto();
    }
}
