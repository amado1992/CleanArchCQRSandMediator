using CleanArchCQRSandMediator.Application.Dtos.Roles;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Roles.Queries.GetUserRoles
{
    public class GetUserRolesQuery : IRequest<RolesResponse>
    {
    }
}
