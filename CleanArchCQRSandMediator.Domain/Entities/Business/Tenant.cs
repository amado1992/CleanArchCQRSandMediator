using CleanArchCQRSandMediator.Domain.Entities.Business.Product;
using CleanArchCQRSandMediator.Domain.Entities.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Business
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Slug: Mandatory, unique, and for friendly URLs
        public string Slug { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
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
