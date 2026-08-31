using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Permissions;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Application.Permissions.Queries.GetUserUnifiedPermissionsQuery
{
    public class GetUserUnifiedPermissionsQueryHandler : IRequestHandler<GetUserUnifiedPermissionsQuery, PermissionsResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetUserUnifiedPermissionsQueryHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PermissionsResponse> Handle(GetUserUnifiedPermissionsQuery request, CancellationToken cancellationToken)
        {
            // 1. Get the user
            var userId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), userId);

            // 2. HashSet to store unique permissions (no duplicates)
            var permissionsSet = new HashSet<string>();

            // 3. Obtain direct user permissions (AspNetUserClaims)
            var userClaims = await _userManager.GetClaimsAsync(user);
            foreach (var claim in userClaims.Where(c => c.Type == "permission"))
            {
                permissionsSet.Add(claim.Value);
            }

            // 4. Retrieve user roles
            var userRoleNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in userRoleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var claim in roleClaims.Where(c => c.Type == "permission"))
                    {
                        permissionsSet.Add(claim.Value);
                    }
                }
            }

            return new PermissionsResponse
            {
                Permissions = permissionsSet.ToList()
            };
        }
    }
}
