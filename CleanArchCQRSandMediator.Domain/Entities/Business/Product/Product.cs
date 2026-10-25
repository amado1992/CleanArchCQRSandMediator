using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business.Product
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? BrandId { get; set; }
        public Brand? Brand { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        /// <summary>
        /// User who created the product. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the product.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
