using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Business;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchCQRSandMediator.Application.Organizations.Commands.CreateOrganization
{
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrganizationCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
        {
            // 1. Normalize the slug (lowercase, no spaces)
            var normalizedSlug = request.Slug.Trim().ToLowerInvariant();

            // 2. Verify that the slug does not exist (is unique)
            var slugExists = await _context.Tenants
                .AnyAsync(o => o.Slug == normalizedSlug, cancellationToken);

            if (slugExists)
                throw new ConflictException($"There is already an organization with the slug '{normalizedSlug}'.");

            // 3. Create the entity
            var organization = new Tenant
            {
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                Slug = normalizedSlug,
                IsActive = request.IsActive
            };

            // 4. Save
            _context.Tenants.Add(organization);
            await _context.SaveChangesAsync(cancellationToken);

            return organization.Id;
        }
    }
}
