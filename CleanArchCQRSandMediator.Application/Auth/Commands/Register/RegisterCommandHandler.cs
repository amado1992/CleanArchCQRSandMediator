using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Business;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchCQRSandMediator.Application.Auth.Commands.Register
{
    /// <summary>
    /// Register user
    /// </summary>
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterCommandHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<int> tenantIds = request.TenantIds;
            var tenants = _context.Tenants.Where(x => !tenantIds.Contains(x.Id)).ToList();

            if (tenants.Count() > 0) throw new NotFoundException(nameof(Tenant), "Id");

            var firstName = request.FirstName;
            var middleName = request.MiddleName;
            var lastName = request.LastName;
            var fullName = $"{firstName} {middleName} {lastName}";

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                FullName = fullName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new ApplicationException($"Error creating user: {string.Join(", ", result.Errors)}");

            // Add roles
            await _userManager.AddToRolesAsync(user, request.RoleNames);

            // Add tenants
            var createdUser = await _userManager.FindByEmailAsync(request.Email);
            foreach (int tenantId in tenantIds)
            {
                var userTenant = new ApplicationUserTenant()
                {
                    ApplicationUserId = createdUser!.Id,
                    TenantId = tenantId
                };

                await _context.ApplicationUserTenant.AddAsync(userTenant);
                await _context.SaveChangesAsync();
            }

            return user.Id;
        }
    }
}
