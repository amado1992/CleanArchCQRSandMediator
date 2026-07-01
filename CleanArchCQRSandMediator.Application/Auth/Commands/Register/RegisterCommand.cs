using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Register
{
    public record RegisterCommand : IRequest<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public IEnumerable<string> RoleNames { get; set; } = new List<string>();
        public IEnumerable<int> TenantIds { get; set; } = new List<int>();
    }
}
