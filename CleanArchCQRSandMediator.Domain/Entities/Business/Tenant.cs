using CleanArchCQRSandMediator.Domain.Common;
using CleanArchCQRSandMediator.Domain.Entities.Business.Product;
using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Tenant : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Slug: Mandatory, unique, and for friendly URLs
        public string Slug { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDefault { get; set; } = false;
        /// <summary>
        /// User who created the tenant. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the tenant.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; } = [];
        public List<ApplicationUserTenant> ApplicationUserTenants { get; } = [];

        public List<Brand> Brands { get; } = [];
        public List<Category> Categories { get; } = [];
        public List<Domain.Entities.Business.Product.Product> Products { get; } = [];
        public List<BranchOffice> BranchOffices { get; } = [];
        public List<Customer> Customers { get; } = [];
        public List<Provider> Providers { get; } = [];
        public List<Purchase> Purchases { get; } = [];
    }
}
