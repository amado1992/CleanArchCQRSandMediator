using CleanArchCQRSandMediator.Domain.Entities.Business;
using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole<int>
    {
        public List<Permission> Permissions { get; } = [];
        public List<ApplicationRolePermission> ApplicationRolePermissions { get; } = [];
    }
}
