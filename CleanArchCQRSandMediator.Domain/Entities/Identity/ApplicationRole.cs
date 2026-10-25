using CleanArchCQRSandMediator.Domain.Entities.Business;
using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<int>
    {
        public List<Permission> Permissions { get; } = [];
        public List<ApplicationRolePermission> ApplicationRolePermissions { get; } = [];

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// User who created the application role. Nullable for seeders/migrations/jobs.
        /// </summary>
        public int? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        /// <summary>
        /// User who last updated the application role.
        /// </summary>
        public int? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
