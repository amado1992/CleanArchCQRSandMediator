using MediatR;
namespace CleanArchCQRSandMediator.Application.Permissions.Queries.GetUserUnifiedPermissionsQuery
{
    public record GetUserUnifiedPermissionsQuery : IRequest<IList<string>>
    {
    }
}
