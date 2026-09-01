using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Roles;
using CleanArchCQRSandMediator.Application.Roles.Queries.GetUserRoles;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Application.Users.Queries.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, RolesResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<RolesResponse> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            // Get the user
            var userId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), userId);

            // Retrieve user roles (unnormalized names)
            var roles = await _userManager.GetRolesAsync(user);

            return new RolesResponse
            {
                Roles = roles.ToList()
            };
        }
    }
}
