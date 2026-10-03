using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Organizations.Commands.DeleteOrganization
{
    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteOrganizationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            // 1. Search for the organization by ID
            var organization = await _context.Tenants
                .FirstOrDefaultAsync(o => o.Id == request.OrganizationId, cancellationToken);

            if (organization == null)
                throw new NotFoundException("The organization was not found.");

            // 2. Soft delete: mark as inactive (recommended for SaaS)
            // organization.IsActive = false;

            // 3. Save changes
            _context.Tenants.Remove(organization);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
