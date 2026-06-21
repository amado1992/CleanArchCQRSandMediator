using CleanArchCQRSandMediator.Domain.Entities.Business;
using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<Tenant> Tenants { get; } = [];
        public List<ApplicationUserTenant> ApplicationUserTenants { get; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
    }
}
