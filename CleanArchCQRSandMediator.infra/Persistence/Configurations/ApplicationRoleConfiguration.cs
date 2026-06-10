using CleanArchCQRSandMediator.Domain.Entities.Business;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            // builder.ToTable("asp_net_roles"); // Specify the exact name of the table in the database

            // many-to-many
            builder.HasMany(e => e.Permissions)
                   .WithMany(e => e.ApplicationRoles)
                   .UsingEntity<ApplicationRolePermission>(
                       r => r.HasOne<Permission>(e => e.Permission).WithMany(e => e.ApplicationRolePermissions).HasForeignKey(e => e.PermissionId).HasPrincipalKey(e => e.Id).OnDelete(DeleteBehavior.Restrict),
                       l => l.HasOne<ApplicationRole>(e => e.ApplicationRole).WithMany(e => e.ApplicationRolePermissions).HasForeignKey(e => e.ApplicationRoleId).HasPrincipalKey(e => e.Id).OnDelete(DeleteBehavior.Restrict),
                       j => j.HasKey(e => e.Id));
        }
    }
}
