using CleanArchCQRSandMediator.Application.Dtos.Users;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Users.Queries.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<UserProfileResponse>
    {
    }
}
