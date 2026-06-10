using CleanArchCQRSandMediator.Domain.Entities.Business;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
           // builder.ToTable("asp_net_users"); // Specify the exact name of the table in the database

            // many-to-many
            builder.HasMany(e => e.Tenants)
                   .WithMany(e => e.ApplicationUsers)
                   .UsingEntity<ApplicationUserTenant>(
                       r => r.HasOne<Tenant>(e => e.Tenant).WithMany(e => e.ApplicationUserTenants).HasForeignKey(e => e.TenantId).HasPrincipalKey(e => e.Id).OnDelete(DeleteBehavior.Restrict),
                       l => l.HasOne<ApplicationUser>(e => e.ApplicationUser).WithMany(e => e.ApplicationUserTenants).HasForeignKey(e => e.ApplicationUserId).HasPrincipalKey(e => e.Id).OnDelete(DeleteBehavior.Restrict),
                       j => j.HasKey(e => e.Id));
        }
    }
}
