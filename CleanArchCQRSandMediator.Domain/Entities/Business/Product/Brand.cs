using CleanArchCQRSandMediator.Domain.Common;

namespace CleanArchCQRSandMediator.Domain.Entities.Business.Product
{
    public class Brand : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<Product> Products { get; } = [];
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;
    }
}
