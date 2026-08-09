using CleanArchCQRSandMediator.Domain.Entities.Business;
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
            CreateTenants(context);
        }

        public static async Task InitializeAsync(
            ApplicationDbContext context,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            // Apply pending migrations
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            await CreateRolesAsync(roleManager);
            await CreateUsersAsync(userManager, roleManager);
            await CreateTenantsAsync(context);
            await CreateCurrenciesAsync(context);
        }

        /// <summary>
        /// Create role async
        /// </summary>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        public static async Task CreateRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            var roles = new[] { "Super administrador", "Dueño", "Miembro" };
            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new ApplicationRole { Name = roleName };
                    await roleManager.CreateAsync(role);
                }
            }
        }

        /// <summary>
        /// Create user async
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static async Task CreateUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            var users = new[]
            {
                new
                {
                    Email = "aramirezamdo1992@gmail.com",
                    FirstName = "Amado",
                    MiddleName = "Rafael",
                    FirstSurname = "Ramírez",
                    SecondSurname = "López",
                    FullName = "Amado Rafael Ramírez López",
                    Password = "Working02026.com",
                    RoleNormalizedName = "SUPER ADMINISTRADOR"
                }
            };

            foreach (var userData in users)
            {
                if (await userManager.FindByEmailAsync(userData.Email) != null)
                    continue;

                var user = new ApplicationUser
                {
                    UserName = userData.Email,
                    Email = userData.Email,
                    FirstName = userData.FirstName,
                    MiddleName = userData.MiddleName,
                    FirstSurname = userData.FirstSurname,
                    SecondSurname = userData.SecondSurname,
                    FullName = userData.FullName,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await userManager.CreateAsync(user, userData.Password);
                if (!result.Succeeded)
                {
                    throw new Exception($"Error creating user {userData.Email}: {string.Join(", ", result.Errors)}");
                }

                // Assign role
                if (!string.IsNullOrEmpty(userData.RoleNormalizedName))
                {
                    var role = await roleManager.FindByNameAsync(userData.RoleNormalizedName);
                    if (role != null)
                    {
                        await userManager.AddToRoleAsync(user, role.Name!);
                    }
                }
            }
        }

        /// <summary>
        /// Create tenants async
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task CreateTenantsAsync(ApplicationDbContext context)
        {
            var tenants = new[]
            {
                new Tenant
                {
                    Name = "Nina Nails Shop",
                    Description = "",
                    Slug = "nina_nails_shop",
                    IsActive = true
                }
            };

            foreach (var tenant in tenants)
            {
                if (!await context.Tenants.AnyAsync(t => t.Slug == tenant.Slug))
                {
                    await context.Tenants.AddAsync(tenant);
                }
            }

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Create currencies async
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task CreateCurrenciesAsync(ApplicationDbContext context)
        {
            var CreatedAt = DateTime.UtcNow;
            var currencies = new List<Currency>
            {
                new Currency { Code = "EUR", Number = 978, Symbol = "€", CreatedAt = CreatedAt },
                new Currency { Code = "USD", Number = 840, Symbol = "$", CreatedAt = CreatedAt },
            };

            foreach (var currency in currencies)
            {
                if (!await context.Currencies.AnyAsync(c => c.Code == currency.Code))
                {
                    await context.Currencies.AddAsync(currency);
                }
            }

            await context.SaveChangesAsync();
        }

        private static void CreateRoles(ApplicationDbContext context)
        {
            var rolSuperAdmin = new ApplicationRole()
            {
                Name = "Super administrador",
                NormalizedName = "SUPER ADMINISTRADOR",
            };

            CreateRole(context, rolSuperAdmin);

            var rolOwner = new ApplicationRole()
            {
                Name = "Dueño",
                NormalizedName = "DUEÑO"
            };

            CreateRole(context, rolOwner);


            var rolMember = new ApplicationRole()
            {
                Name = "Miembro",
                NormalizedName = "MIEMBRO"
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
        private static void CreateUser(ApplicationDbContext context, string email, string firstName, string middleName, string firstSurname, string secondSurname, string fullName, string password, string rolNormalizedName)
        {
            var user = new ApplicationUser();
            var passwordHasher = new PasswordHasher<ApplicationUser>().HashPassword(user, password);
            var rol = context.Roles.SingleOrDefault(x => x.NormalizedName == rolNormalizedName);

            if (!context.Users.Any(x => x.Email == email))
            {
                user.FirstName = firstName;
                user.MiddleName = middleName;
                user.FirstSurname = firstSurname;
                user.SecondSurname = secondSurname;
                user.FullName = fullName;
                user.UserName = email;
                user.NormalizedUserName = email.ToUpper();
                user.Email = email;
                user.NormalizedEmail = email.ToUpper();
                user.PasswordHash = passwordHasher;
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.IsActive = true;
                user.CreatedAt = DateTime.UtcNow;

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
            CreateUser(context, "aramirezamdo1992@gmail.com", "Amado", "Rafael", "Ramírez", "López", "Amado Rafael Ramírez López", "Working02026.com", "SUPER_ADMIN");
        }

        public static void CreateTenants(ApplicationDbContext context)
        {
            var tenant = new Tenant()
            {
                Name = "Nina Nails Shop",
                Description = "",
                Slug = "nina_nails_shop",
                IsActive = true
            };

            CreateTenant(context, tenant);
        }

        /// <summary>
        /// Create tenant 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tenant"></param>
        private static void CreateTenant(ApplicationDbContext context, Tenant tenant)
        {
            if (!context.Tenants.Any(x => x.Slug == tenant.Slug))
            {
                context.Tenants.Add(tenant);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Currencies
        /// </summary>
        /// <param name="context"></param>
        public static void CreateCurrencies(ApplicationDbContext context)
        {
            List<Currency> currencies = new List<Currency>() {
                new Currency {Code = "EUR", Number = 978, Symbol = "€"},
                new Currency {Code = "USD", Number = 840, Symbol = "$"},
                new Currency {Code = "JPY", Number = 392, Symbol = "¥"},
                new Currency {Code = "GBP", Number = 826, Symbol = "£"},
                new Currency {Code = "AUD", Number = 036, Symbol = "$"},
                new Currency {Code = "CAD", Number = 124, Symbol = "$"},
                new Currency {Code = "CHF", Number = 756, Symbol = "Fr"},
                new Currency {Code = "CNY", Number = 156, Symbol = "¥"},
                new Currency {Code = "SEK", Number = 752, Symbol = "kr"},
                new Currency {Code = "NZD", Number = 554, Symbol = "$"},
                new Currency {Code = "HKD", Number = 344, Symbol = "$"}, // Dólar de Hong Kong
                new Currency {Code = "SGD", Number = 702, Symbol = "$"}, // Dólar de Singapur
                new Currency {Code = "INR", Number = 356, Symbol = "₹"}, // Rupia india
                new Currency {Code = "RUB", Number = 643, Symbol = "₽"}, // Rublo ruso
                new Currency {Code = "MXN", Number = 484, Symbol = "$"}, // Peso mexicano
                new Currency {Code = "TRY", Number = 949, Symbol = "₺"}, // Lira turca
                new Currency {Code = "SAR", Number = 682, Symbol = "﷼"}, // Riyal saudí
                new Currency {Code = "BRL", Number = 986, Symbol = "R$"}, // Real brasileño
                new Currency {Code = "ZAR", Number = 710, Symbol = "R"}, // Rand sudafricano
                new Currency {Code = "KRW", Number = 410, Symbol = "₩"}, // Won surcoreano
                new Currency {Code = "DKK", Number = 208, Symbol = "kr"}, // Corona danesa
                new Currency {Code = "PLN", Number = 985, Symbol = "zł"}, // Zloty polaco
                new Currency {Code = "TWD", Number = 901, Symbol = "NT$"}, // Nuevo dólar taiwanés
                new Currency {Code = "THB", Number = 764, Symbol = "฿"}, // Baht tailandés
                new Currency {Code = "MYR", Number = 458, Symbol = "RM"}, // Ringgit malayo
                new Currency {Code = "CZK", Number = 203, Symbol = "Kč"}, // Corona checa
                new Currency {Code = "ILS", Number = 376, Symbol = "₪"}, // Nuevo shekel israelí
                new Currency {Code = "HUF", Number = 348, Symbol = "Ft"}, // Forinto húngaro
                new Currency {Code = "CLP", Number = 152, Symbol = "$"}, // Peso chileno
                new Currency {Code = "PHP", Number = 608, Symbol = "₱"}, // Peso filipino
                new Currency {Code = "IDR", Number = 360, Symbol = "Rp"}, // Rupia indonesia
                new Currency {Code = "RON", Number = 946, Symbol = "lei"}, // Leu rumano
                new Currency {Code = "AED", Number = 784, Symbol = "درهم"}, // Dírham de los Emiratos Árabes Unidos
                new Currency {Code = "COP", Number = 170, Symbol = "$"}, // Peso colombiano
                new Currency {Code = "PEN", Number = 604, Symbol = "S/."}, // Nuevo Sol peruano
                new Currency {Code = "ARS", Number = 032, Symbol = "$"}, // Peso argentino
                new Currency {Code = "EGP", Number = 818, Symbol = "E£"}, // Libra egipcia
                new Currency {Code = "KZT", Number = 398, Symbol = "₸"}, // Tenge kazajo
                new Currency {Code = "QAR", Number = 634, Symbol = "ر.ق"}, // Riyal qatarí
                new Currency {Code = "VND", Number = 704, Symbol = "₫"}, // Dong vietnamita
                new Currency {Code = "UAH", Number = 980, Symbol = "₴"}, // Grivna ucraniana
                new Currency {Code = "OMR", Number = 512, Symbol = "ر.ع."}, // Riyal omaní
                new Currency {Code = "KWD", Number = 414, Symbol = "د.ك"}, // Dinar kuwaití
                new Currency {Code = "BHD", Number = 048, Symbol = ".د.ب"}, // Dinar bahreiní
                new Currency {Code = "JOD", Number = 400, Symbol = "د.ا"}, // Dinar jordano
                new Currency {Code = "BGN", Number = 975, Symbol = "лв"}, // Lev búlgaro
                new Currency {Code = "HRK", Number = 191, Symbol = "kn"}, // Kuna croata
                new Currency {Code = "UYU", Number = 858, Symbol = "$U"} // Peso uruguayo
            };

            foreach (var currency in currencies)
            {
                if (!context.Currencies.Any(x => x.Code == currency.Code))
                    context.Currencies.Add(currency);
                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}
