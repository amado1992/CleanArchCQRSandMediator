using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CleanArchCQRSandMediator.Application.Permissions.Commands.AssignPermissionsToRole
{
    public class AssignPermissionsToRoleCommandHandler : IRequestHandler<AssignPermissionsToRoleCommand>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AssignPermissionsToRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
        {
            // 1. Get role by ID
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            if (role == null)
                throw new NotFoundException(nameof(ApplicationRole), request.RoleId);

            // 2. Obtain current role claims (existing permissions)
            var existingClaims = await _roleManager.GetClaimsAsync(role);
            var existingPermissions = existingClaims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToHashSet();

            // 3. Standardize and filter existing permissions
            var normalizedPermissions = request.PermissionNames
                .Select(p => p.Trim().ToLower())
                .Where(p => !existingPermissions.Contains(p))
                .ToList();

            if (normalizedPermissions.Count == 0)
                return; // There are no new permissions to add

            // 4. Add each new permission as a claim to the role
            foreach (var perm in normalizedPermissions)
            {
                var claim = new Claim("permission", perm);
                await _roleManager.AddClaimAsync(role, claim);
            }
        }
    }
}
