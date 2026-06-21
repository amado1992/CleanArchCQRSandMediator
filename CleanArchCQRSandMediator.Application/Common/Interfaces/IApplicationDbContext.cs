using CleanArchCQRSandMediator.Domain.Entities.Business;
using CleanArchCQRSandMediator.Domain.Entities.Business.Product;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchCQRSandMediator.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        // Business
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Domain.Entities.Business.Action> Actions { get; set; }
        public DbSet<ApplicationRolePermission> ApplicationRolePermission { get; set; }
        public DbSet<ApplicationUserTenant> ApplicationUserTenant { get; set; }
        public DbSet<Module> Modules { get; set; }
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
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<IdentityUserRole<int>> ApplicationUserRoles { get; set; }

        public Task<int> SaveChangesAsync();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        // Both methods do the same thing
        public void SaveChangesSynchronous();
        public void SaveChanges();
    }
}
