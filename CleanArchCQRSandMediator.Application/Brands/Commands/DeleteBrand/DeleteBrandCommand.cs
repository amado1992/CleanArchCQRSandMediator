using MediatR;

namespace CleanArchCQRSandMediator.Application.Brands.Commands.DeleteBrand
{
    public record DeleteBrandCommand : IRequest<Unit>
    {
        public int BrandId { get; set; }
    }
}
