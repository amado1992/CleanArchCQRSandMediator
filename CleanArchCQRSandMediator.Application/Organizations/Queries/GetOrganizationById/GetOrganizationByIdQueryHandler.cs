using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Organizations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;


namespace CleanArchCQRSandMediator.Application.Organizations.Queries.GetOrganizationById
{
    public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public GetOrganizationByIdQueryHandler(IApplicationDbContext context, IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<OrganizationDto> Handle(
            GetOrganizationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var organization = await _context.Tenants
                .AsNoTracking()
                .Where(o => o.Id == request.OrganizationId)
                .Select(o => new OrganizationDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    Slug = o.Slug,
                    IsActive = o.IsActive,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (organization == null)
                throw new NotFoundException(_localizer["Organization_Not_Found"]);

            return organization;
        }
    }
}
