using CleanArchCQRSandMediator.Domain.Entities.Identity;
using CleanArchCQRSandMediator.infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.infra.Persistence.InitialData
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            CreateRoles(context);
            CreateUsers(context);
        }

        private static void CreateRoles(ApplicationDbContext context)
        {
            var rolSuperAdmin = new ApplicationRole()
            {
                Name = "Super administrador",
                NormalizedName = "SUPER_ADMIN",
            };

            CreateRole(context, rolSuperAdmin);

            var rolOwner = new ApplicationRole()
            {
                Name = "Dueño",
                NormalizedName = "OWNER"
            };

            CreateRole(context, rolOwner);


            var rolMember = new ApplicationRole()
            {
                Name = "Miembro",
                NormalizedName = "MEMBER"
            };

            CreateRole(context, rolMember);
        }

        /// <summary>
        /// Create roles
        /// </summary>
        /// <param name="context"></param>
        /// <param name="role"></param>
        private static void CreateRole(ApplicationDbContext context, ApplicationRole role)
        {
            if (!context.Roles.Any(x => x.Name == role.Name))
            {
                context.Roles.Add(role);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="rolName"></param>
        private static void CreateUser(ApplicationDbContext context, string email, string firstName, string middleName, string lastName, string fullName, string password, string rolNormalizedName)
        {
            var user = new ApplicationUser();
            var passwordHasher = new PasswordHasher<ApplicationUser>().HashPassword(user, password);
            var rol = context.Roles.SingleOrDefault(x => x.NormalizedName == rolNormalizedName);

            if (!context.Users.Any(x => x.Email == email))
            {
                user.FirstName = firstName;
                user.MiddleName = middleName;
                user.LastName = lastName;
                user.FullName = fullName;
                user.UserName = email;
                user.NormalizedUserName = email.ToUpper();
                user.Email = email;
                user.NormalizedEmail = email.ToUpper();
                user.PasswordHash = passwordHasher;
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();

                context.Users.Add(user);
                context.SaveChanges();

                if (rol != null)
                {
                    context.UserRoles.Add(new IdentityUserRole<int>
                    {
                        RoleId = rol.Id,
                        UserId = user.Id
                    });
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Create users
        /// </summary>
        /// <param name="context"></param>
        private static void CreateUsers(ApplicationDbContext context)
        {
            CreateUser(context, "aramirezamdo1992@gmail.com", "Amado", "Rafael", "Ramírez López", "Amado Rafael Ramírez López", "Working02026.com", "SUPER_ADMIN");
        }
    }
}
