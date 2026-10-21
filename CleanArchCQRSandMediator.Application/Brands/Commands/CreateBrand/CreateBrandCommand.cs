using MediatR;

namespace CleanArchCQRSandMediator.Application.Brands.Commands.CreateBrand
{
    public record CreateBrandCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int TenantId { get; set; }
    }
}
