using CleanArchCQRSandMediator.Application.Dtos.Auth;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Login
{
    // public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

    /// <summary>
    /// Login command
    /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records">C# record types</see>
    /// </summary>
    public record LoginCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
