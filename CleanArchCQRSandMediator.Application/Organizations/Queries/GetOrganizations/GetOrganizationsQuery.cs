using CleanArchCQRSandMediator.Application.Common.Models;
using CleanArchCQRSandMediator.Application.Dtos.Organizations;
using MediatR;

namespace CleanArchCQRSandMediator.Application.Organizations.Queries.GetOrganizations
{
    public record GetOrganizationsQuery : IRequest<PagedResult<OrganizationDto>>
    {
       public OrganizationFilterDto Filter { get; set; } = new OrganizationFilterDto();
    }
}
