using CleanArchCQRSandMediator.Domain.Enums;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Register
{
    public record RegisterCommand : IRequest<int>
    {
        public string FirstName { get; init; } = string.Empty;
        public string? MiddleName { get; init; } = null;
        public string FirstSurname { get; init; } = string.Empty;
        public string SecondSurname { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string CellPhone { get; set; } = string.Empty;
        public string? WhatsApp { get; set; } = null;
        public string? Address { get; set; } = null;
        public string Sex { get; set; } = string.Empty;
        public IEnumerable<string> RoleNames { get; init; } = new List<string>();
        public IEnumerable<int> TenantIds { get; init; } = new List<int>();
    }
}
