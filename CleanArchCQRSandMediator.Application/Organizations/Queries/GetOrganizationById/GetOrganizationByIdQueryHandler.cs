using AutoMapper;
using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Application.Dtos.Organizations;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace CleanArchCQRSandMediator.Application.Organizations.Queries.GetOrganizationById
{
    public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetOrganizationByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrganizationDto> Handle(
            GetOrganizationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var organization = await _context.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == request.OrganizationId, cancellationToken);

            if (organization == null)
                throw new NotFoundException("The organization was not found.");

            return _mapper.Map<OrganizationDto>(organization);
        }
    }
}
