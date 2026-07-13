using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CleanArchCQRSandMediator.Application.Permissions.Commands.AssignPermissionToUser
{
    public class AssignPermissionToUserCommandHandler : IRequestHandler<AssignPermissionToUserCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignPermissionToUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(AssignPermissionToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), request.UserId);

            // Obtain current user permissions (to avoid duplicates)
            var existingClaims = await _userManager.GetClaimsAsync(user);
            var existingPermissions = existingClaims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToHashSet();

            // Standardize and filter existing permissions
            var normalizedPermissions = request.PermissionNames
                .Select(p => p.Trim().ToLower())
                .Where(p => !existingPermissions.Contains(p))
                .ToList();

            // There are no new permissions to add
            if (normalizedPermissions.Count == 0)
                return;

            // Create the claims and add them in batch
            var claims = normalizedPermissions.Select(p => new Claim("permission", p));
            foreach (var claim in claims)
            {
                await _userManager.AddClaimAsync(user, claim);
            }
        }
    }
}
