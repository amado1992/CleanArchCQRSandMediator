using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Organizations.Commands.UpdateOrganization
{
    public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateOrganizationCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            // 1. Normalize the slug (lowercase, no spaces)
            var normalizedSlug = request.Slug.Trim().ToLowerInvariant();

            // 2. Search for the organization by ID
            var organization = await _context.Tenants
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (organization == null)
                throw new NotFoundException("The organization was not found.");

            // 3. Verify that the new slug is not in use by another organization.
            var slugExists = await _context.Tenants
                .AnyAsync(o => o.Slug == normalizedSlug && o.Id != request.Id, cancellationToken);

            if (slugExists)
                throw new ConflictException($"There is already an organization with the slug '{normalizedSlug}'.");

            // 4. Update the properties
            organization.Name = request.Name.Trim();
            organization.Description = request.Description?.Trim() ?? string.Empty;
            organization.Slug = normalizedSlug;
            organization.IsActive = request.IsActive;

            // 5. Save changes
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
