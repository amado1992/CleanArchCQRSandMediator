using CleanArchCQRSandMediator.Application.Common.Exceptions;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Business;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using CleanArchCQRSandMediator.Domain.Enums;
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
            var firstName = request.FirstName;
            var middleName = request.MiddleName;
            var firstSurname = request.FirstSurname;
            var secondSurname = request.SecondSurname;
            var fullName = $"{firstName} {middleName} {firstName} {secondSurname}";
            var email = request.Email;
            var userName = request.UserName;

            // Verify that the username does not exist.
            if (await _userManager.FindByNameAsync(userName) != null)
                throw new ConflictException($"The username '{userName}' it is already in use.");

            // Verify that the email does not exist.
            if (await _userManager.FindByEmailAsync(email) != null)
                throw new ConflictException($"The email '{email}' it is already registered.");

            IEnumerable<int> tenantIds = request.TenantIds;
            var tenants = _context.Tenants.Where(x => !tenantIds.Contains(x.Id)).ToList();
            if (tenants.Count() > 0) throw new NotFoundException(nameof(Tenant), "Id");

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FirstName = firstName,
                MiddleName = middleName,
                FirstSurname = firstSurname,
                SecondSurname = secondSurname,
                FullName = fullName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CellPhone = request.CellPhone,
                WhatsApp = request.WhatsApp,
                Address = request.Address,
                Sex = Enum.Parse<Sex>(request.Sex)
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new IdentityException("Error creating user", result.Errors);

            // Add roles
            await _userManager.AddToRolesAsync(user, request.RoleNames);

            // Add tenants
            var createdUser = await _userManager.FindByEmailAsync(email);
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
