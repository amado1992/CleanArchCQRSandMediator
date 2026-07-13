using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}
