using CleanArchCQRSandMediator.Application.Dtos.Permissions;
using MediatR;
namespace CleanArchCQRSandMediator.Application.Permissions.Queries.GetUserUnifiedPermissionsQuery
{
    public record GetUserUnifiedPermissionsQuery : IRequest<PermissionsResponse>
    {
    }
}
