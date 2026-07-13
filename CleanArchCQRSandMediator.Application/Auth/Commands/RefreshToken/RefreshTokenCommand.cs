using CleanArchCQRSandMediator.Application.Dtos.Auth;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<LoginResponse>
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
    }
}
