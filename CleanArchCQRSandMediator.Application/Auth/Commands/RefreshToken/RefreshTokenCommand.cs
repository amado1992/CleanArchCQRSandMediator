using CleanArchCQRSandMediator.Application.Dtos.Auth;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<LoginResponse>
    {
        public string RefreshToken { get; init; } = string.Empty;
    }
}
