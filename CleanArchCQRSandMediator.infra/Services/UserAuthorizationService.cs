using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.infra.Services
{
    /// <summary>
    /// Servicio de autorización que combina datos del JWT con consultas a la BD.
    /// Se usa cuando se necesita verificar el estado actual del usuario (roles, tenant default).
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
        /// Verifica si el usuario actual es Super Administrador.
        /// </summary>
        public async Task<bool> IsSuperAdminAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.GetUserId();
            return await IsSuperAdminAsync(userId, cancellationToken);
        }

        /// <summary>
        /// Verifica si un usuario específico es Super Administrador.
        /// </summary>
        public async Task<bool> IsSuperAdminAsync(int userId, CancellationToken cancellationToken = default)
        {
            var roles = await GetRolesAsync(userId, cancellationToken);
            return roles.Contains(SuperAdminRole, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Verifica si un usuario tiene un rol específico (case-insensitive).
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
        /// Obtiene los roles de un usuario desde la BD.
        /// Devuelve lista vacía si el usuario no existe.
        /// </summary>
        public async Task<IList<string>> GetRolesAsync(int userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return [];

            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Obtiene los roles del usuario actual.
        /// </summary>
        public async Task<IList<string>> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.GetUserId();
            return await GetRolesAsync(userId, cancellationToken);
        }

        /// <summary>
        /// Obtiene el TenantId del tenant default del usuario actual.
        /// Devuelve null si no pertenece al tenant default o no hay default configurado.
        /// </summary>
        public async Task<int?> GetDefaultTenantIdAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.GetUserId();
            return await GetDefaultTenantIdAsync(userId, cancellationToken);
        }

        /// <summary>
        /// Obtiene el TenantId del tenant default (IsDefault = true) al que pertenece el usuario.
        /// Devuelve null si no hay default, está inactivo, o el usuario no pertenece a él.
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
    }
}