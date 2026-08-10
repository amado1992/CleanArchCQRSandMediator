using CleanArchCQRSandMediator.Application.Authorization;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using CleanArchCQRSandMediator.infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace CleanArchCQRSandMediator.infra.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // If the user is not authenticated, it fails
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Fail();
                return;
            }

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out var userInt))
            {
                context.Fail();
                return;
            }

            // 1. Verify if the user is an super administrator (permissions can be bypassed)
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                context.Fail();
                return;
            }

            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "Super administrador");
            if (isSuperAdmin)
            {
                context.Succeed(requirement);
                return;
            }

            // 2. Verify tenant
            var tenantIds = await _context.ApplicationUserTenant
                .Where(x => x.ApplicationUserId == int.Parse(userId))
                .Select(uc => uc.TenantId)
                .ToListAsync();
            
            if (tenantIds.Count == 0)
            {
                context.Fail();
                return;
            }

            // 3. Verify direct user permissions (from AspNetUserClaims)
            var userPermissions = await _context.UserClaims
                .Where(uc => uc.UserId == userInt && uc.ClaimType == "permission")
                .Select(uc => uc.ClaimValue)
                .ToListAsync();

            // 4. Verify user role permissions (from AspNetRoleClaims)

            // Get the user role IDs
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles
                .Where(r => userRoleNames.Contains(r.Name!))
                .Select(r => r.Id)
                .ToListAsync();

            // Obtain the permissions for those roles from AspNetRoleClaims
            var rolePermissions = await _context.RoleClaims
                .Where(rc => roles.Contains(rc.RoleId) && rc.ClaimType == "permission")
                .Select(rc => rc.ClaimValue)
                .ToListAsync();

            // Combine all (different) permissions
            var allPermissions = userPermissions
                .Union(rolePermissions)
                .Distinct()
                .ToList();

            // Check if the required permit is on the list
            var requiredPermission = requirement.GetPermissionName();
            var hasPermission = allPermissions.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
