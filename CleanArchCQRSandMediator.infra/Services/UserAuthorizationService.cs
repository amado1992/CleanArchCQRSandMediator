using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.infra.Services
{
    /// <summary>
    /// Authorization service that combines data from the JWT with database queries.
    /// It is used when it is necessary to verify the user's current status (roles, default tenant).
    /// </summary>
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private const string SuperAdminRole = "Super administrador";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _context;

        public UserAuthorizationService(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IApplicationDbContext context)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _context = context;
        }

        /// <summary>
        /// Check whether the current user is a Super Administrator.
        /// </summary>
        public async Task<bool> IsSuperAdminAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.GetUserId();
            return await IsSuperAdminAsync(userId, cancellationToken);
        }

        /// <summary>
        /// Check whether a specific user is a Super Administrator.
        /// </summary>
        public async Task<bool> IsSuperAdminAsync(int userId, CancellationToken cancellationToken = default)
        {
            var roles = await GetRolesAsync(userId, cancellationToken);
            return roles.Contains(SuperAdminRole, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks whether a user has a specific role (case-insensitive).
        /// </summary>
        public async Task<bool> IsInRoleAsync(
            int userId,
            string roleName,
            CancellationToken cancellationToken = default)
        {
            var roles = await GetRolesAsync(userId, cancellationToken);
            return roles.Contains(roleName, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Retrieves a user's roles from the database.
        /// Returns an empty list if the user does not exist.
        /// </summary>
        public async Task<IList<string>> GetRolesAsync(int userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return [];

            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Retrieves the roles of the current user.
        /// </summary>
        public async Task<IList<string>> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.GetUserId();
            return await GetRolesAsync(userId, cancellationToken);
        }

        /// <summary>
        /// Retrieves the TenantId of the current user's default tenant.
        /// Returns null if it does not belong to the default tenant or if no default is configured.
        /// </summary>
        public async Task<int?> GetDefaultTenantIdAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.GetUserId();
            return await GetDefaultTenantIdAsync(userId, cancellationToken);
        }

        /// <summary>
        /// Retrieves the TenantId of the default tenant (IsDefault = true) to which the user belongs.
        /// Returns null if there is no default, it is inactive, or the user does not belong to it.
        /// </summary>
        public async Task<int?> GetDefaultTenantIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.Tenants
                .AsNoTracking()
                .Where(t => t.IsDefault
                         && t.IsActive
                         && t.ApplicationUserTenants.Any(aut => aut.ApplicationUserId == userId))
                .Select(t => (int?)t.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Checks whether a user belongs to a specific tenant (by querying the database).
        /// </summary>
        public async Task<bool> UserBelongsToTenantAsync(
            int userId,
            int tenantId,
            CancellationToken cancellationToken = default)
        {
            return await _context.ApplicationUserTenant
                .AsNoTracking()
                .AnyAsync(aut => aut.ApplicationUserId == userId
                              && aut.TenantId == tenantId,
                         cancellationToken);
        }

        /// <summary>
        /// Check whether the current user belongs to a specific tenant.
        /// </summary>
        public async Task<bool> CurrentUserBelongsToTenantAsync(
            int tenantId,
            CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.GetUserId();
            return await UserBelongsToTenantAsync(userId, tenantId, cancellationToken);
        }

        /// <summary>
        /// Verify that the current user belongs to the specified tenant.
        /// Throws <see cref="ForbiddenException"/> if it doesn't belong.
        /// </summary>
        public async Task EnsureCurrentUserBelongsToTenantAsync(
            int tenantId,
            CancellationToken cancellationToken = default)
        {
            var belongs = await CurrentUserBelongsToTenantAsync(tenantId, cancellationToken);
            if (!belongs)
                throw new ForbiddenException("You do not have access to this tenant.");
        }

        /// <summary>
        /// Verify that the current user can access the specified tenant.
        /// Super Admin: always has access. Standard user: must belong to the tenant.
        /// </summary>
        public async Task EnsureCanAccessTenantAsync(
            int tenantId,
            CancellationToken cancellationToken = default)
        {
            // Super Admin: full access (without membership validation).
            var isSuperAdmin = await IsSuperAdminAsync(cancellationToken);
            if (isSuperAdmin)
                return;

            // Normal user: must belong to the tenant
            await EnsureCurrentUserBelongsToTenantAsync(tenantId, cancellationToken);
        }
    }
}