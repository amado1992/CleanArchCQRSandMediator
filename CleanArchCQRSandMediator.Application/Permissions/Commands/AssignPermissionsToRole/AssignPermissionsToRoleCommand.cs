using MediatR;

namespace CleanArchCQRSandMediator.Application.Permissions.Commands.AssignPermissionsToRole
{
    public record AssignPermissionsToRoleCommand : IRequest
    {
        public int RoleId { get; set; }
        public IEnumerable<string> PermissionNames { get; set; } = new List<string>();
    }
}
