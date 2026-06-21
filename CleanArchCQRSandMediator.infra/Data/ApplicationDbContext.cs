using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Business;
using CleanArchCQRSandMediator.Domain.Entities.Business.Product;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CleanArchCQRSandMediator.infra.Data
{
    // Solution without Identity
    // public class ApplicationDbContext : DbContext, IApplicationDbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        // Business
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Domain.Entities.Business.Action> Actions { get; set; }
        public DbSet<ApplicationRolePermission> ApplicationRolePermission { get; set; }
        public DbSet<ApplicationUserTenant> ApplicationUserTenant { get; set; }
        public DbSet<Domain.Entities.Business.Module> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<BranchOffice> BranchOffices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        // Products
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        // Identity
        public DbSet<ApplicationUser> ApplicationUsers { get => Users; set => Users = value; }
        public DbSet<ApplicationRole> ApplicationRoles { get => Roles; set => Roles = value; }
        public DbSet<IdentityUserRole<int>> ApplicationUserRoles
        {
            get => UserRoles;
            set => UserRoles = value;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        void IApplicationDbContext.SaveChanges()
        {
            base.SaveChanges();
        }

        public void SaveChangesSynchronous()
        {
            base.SaveChanges();
        }

        async Task<int> IApplicationDbContext.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            /*builder.Entity<ApplicationUser>(b => b.ToTable("asp_net_users"));
            builder.Entity<ApplicationRole>(b => b.ToTable("asp_net_roles"));
            builder.Entity<IdentityUserRole<int>>(b => b.ToTable("asp_net_users_roles"));
            builder.Entity<IdentityRoleClaim<int>>(b => b.ToTable("asp_net_role_claims"));
            builder.Entity<IdentityUserClaim<int>>(b => b.ToTable("asp_net_user_claims"));
            builder.Entity<IdentityUserLogin<string>>().HasNoKey().ToTable("asp_net_user_logins");
            builder.Entity<IdentityUserToken<string>>().HasNoKey().ToTable("asp_net_user_tokens");*/
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
