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
    internal class PermissionHandler : AuthorizationHandler<PermissionRequirement>
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

            // Get the user tenant from the claim or from the header
            var tenantId = context.User.FindFirst("tenant_id")?.Value;
            if (string.IsNullOrEmpty(tenantId) || !Guid.TryParse(tenantId, out var tenantGuid))
            {
                // If there is no tenant, try to obtain it from the header (fallback)
                tenantId = _httpContextAccessor.HttpContext?.Request.Headers["X-TenantId"].ToString();
                if (string.IsNullOrEmpty(tenantId) || !Guid.TryParse(tenantId, out tenantGuid))
                {
                    context.Fail();
                    return;
                }
            }

            // 1. Verify if the user is an administrator (permissions can be bypassed)
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                context.Fail();
                return;
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                context.Succeed(requirement);
                return;
            }

            // 2. Verify direct user permissions (from AspNetUserClaims)
            var userPermissions = await _context.UserClaims
                .Where(uc => uc.UserId == userInt && uc.ClaimType == "permission")
                .Select(uc => uc.ClaimValue)
                .ToListAsync();

            // 3. Verify user role permissions (from AspNetRoleClaims)

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
