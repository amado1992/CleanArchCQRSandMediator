using CleanArchCQRSandMediator.Domain.Entities.Business;
using CleanArchCQRSandMediator.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the middle name.
        /// </summary>
        public string? MiddleName { get; set; } = null;

        /// <summary>
        /// Gets or sets the first surname.
        /// </summary>
        public string FirstSurname { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the second surname.
        /// </summary>
        public string SecondSurname { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string CellPhone { get; set; } = string.Empty;
        public string? WhatsApp { get; set; } = null;
        public string? Address { get; set; } = null;
        public Sex Sex { get; set; }
        public List<Tenant> Tenants { get; } = [];
        public List<ApplicationUserTenant> ApplicationUserTenants { get; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
    }
}
