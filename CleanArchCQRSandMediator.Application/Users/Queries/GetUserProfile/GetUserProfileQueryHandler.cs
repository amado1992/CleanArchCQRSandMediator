using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Users;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Application.Users.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<UserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), userId);

            return new UserProfileResponse
            {
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                FirstSurname = user.FirstSurname,
                SecondSurname = user.SecondSurname,
                FullName = user.FullName,
                CellPhone = user.CellPhone,
                WhatsApp = user.WhatsApp,
                Address = user.Address,
                Email = user.Email!,
                Username = user.UserName!
            };
        }
    }
}
