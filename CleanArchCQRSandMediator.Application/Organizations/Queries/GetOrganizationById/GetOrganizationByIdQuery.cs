using CleanArchCQRSandMediator.Application.Dtos.Organizations;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Organizations.Queries.GetOrganizationById
{
    public record GetOrganizationByIdQuery : IRequest<OrganizationDto>
    {
        public int OrganizationId { get; set; }
    }
}
