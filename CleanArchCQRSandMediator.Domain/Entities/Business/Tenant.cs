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

    }
}
