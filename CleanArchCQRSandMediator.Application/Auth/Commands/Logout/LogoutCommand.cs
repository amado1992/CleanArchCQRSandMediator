using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
