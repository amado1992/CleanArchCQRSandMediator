using MediatR;

namespace CleanArchCQRSandMediator.Application.Permissions.Commands.AssignPermissionToUser
{
    public class AssignPermissionToUserCommand : IRequest
    {
        public int UserId { get; set; }
        public IEnumerable<string> PermissionNames { get; set; } = new List<string>();
    }
}
