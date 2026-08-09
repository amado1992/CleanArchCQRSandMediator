using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Register
{
    public record RegisterCommand : IRequest<int>
    {
        public string FirstName { get; init; } = string.Empty;
        public string MiddleName { get; init; } = string.Empty;
        public string FirstSurname { get; init; } = string.Empty;
        public string SecondSurname { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public IEnumerable<string> RoleNames { get; init; } = new List<string>();
        public IEnumerable<int> TenantIds { get; init; } = new List<int>();
    }
}
