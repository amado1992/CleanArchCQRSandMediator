namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface IUserAuthorizationService
    {
        /// <summary>
        /// Check whether the current user (from the token) is a Super Administrator.
        /// Query the database to reflect role changes immediately.
        /// </summary>
        Task<bool> IsSuperAdminAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Check whether a specific user is a Super Administrator.
        /// </summary>
        Task<bool> IsSuperAdminAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check whether a specific user is a Super Administrator.
        /// </summary>
        Task<bool> IsInRoleAsync(int userId, string roleName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all roles for a user (by querying the database).
        /// </summary>
        Task<IList<string>> GetRolesAsync(int userId, CancellationToken cancellationToken = default);

        Task<IList<string>> GetRolesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the user's default TenantId (marked with IsDefault = true).
        /// If there is no default, it returns the user's first active tenant.
        /// Returns null if the user does not belong to any active tenant.
        /// </summary>
        Task<int?> GetDefaultTenantIdAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the default TenantId for the current user (marked with IsDefault = true).
        /// If there is no default, it returns the user's first active tenant.
        /// Returns null if the user does not belong to any active tenant.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int?> GetDefaultTenantIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks whether a user belongs to a specific tenant.
        /// </summary>
        Task<bool> UserBelongsToTenantAsync(
            int userId,
            int tenantId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Check whether the current user belongs to a specific tenant.
        /// </summary>
        Task<bool> CurrentUserBelongsToTenantAsync(
            int tenantId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verify that the current user belongs to the specified tenant.
        /// Lanza <see cref="ForbiddenException"/> si no pertenece.
        /// </summary>
        Task EnsureCurrentUserBelongsToTenantAsync(
            int tenantId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verify that the user can access the indicated tenant based on their role:
        /// - Super Admin: always has permission (without verifying membership).
        /// - Normal user: must belong to the tenant.
        ///   Throws a <see cref="ForbiddenException"/> if access is not possible.
        /// </summary>
        Task EnsureCanAccessTenantAsync(
            int tenantId,
            CancellationToken cancellationToken = default);
    }
}
