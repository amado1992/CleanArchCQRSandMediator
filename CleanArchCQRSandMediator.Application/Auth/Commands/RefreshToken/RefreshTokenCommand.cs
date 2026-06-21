using CleanArchCQRSandMediator.Application.Dtos.Auth;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<LoginResponse>
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
