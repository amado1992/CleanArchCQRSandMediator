using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Logout
{
    public record LogoutCommand : IRequest;
}
