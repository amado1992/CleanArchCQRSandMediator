using MediatR;

namespace CleanArchCQRSandMediator.Application.Organizations.Commands.DeleteOrganization
{
    public record DeleteOrganizationCommand : IRequest<Unit>
    {
        public int OrganizationId { get; set; }
    }
}
