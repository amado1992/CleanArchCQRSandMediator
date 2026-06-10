using CleanArchCQRSandMediator.Domain.Entities.Business;
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
