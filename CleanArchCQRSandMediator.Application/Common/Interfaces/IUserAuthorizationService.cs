namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface IUserAuthorizationService
    {
        /// <summary>
        /// Verifica si el usuario actual (del token) es Super Administrador.
        /// Consulta la BD para reflejar cambios de rol inmediatamente.
        /// </summary>
        Task<bool> IsSuperAdminAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica si un usuario específico es Super Administrador.
        /// </summary>
        Task<bool> IsSuperAdminAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica si un usuario tiene un rol específico (consultando BD).
        /// </summary>
        Task<bool> IsInRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene todos los roles de un usuario (consultando BD).
        /// </summary>
        Task<IList<string>> GetRolesAsync(int userId, CancellationToken cancellationToken = default);

        Task<IList<string>> GetRolesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene el TenantId por defecto del usuario (marcado con IsDefault = true).
        /// Si no tiene default, devuelve el primer tenant activo del usuario.
        /// Retorna null si el usuario no pertenece a ningún tenant activo.
        /// </summary>
        Task<int?> GetDefaultTenantIdAsync(int userId, CancellationToken cancellationToken = default);

        Task<int?> GetDefaultTenantIdAsync(CancellationToken cancellationToken = default);
    }
}
