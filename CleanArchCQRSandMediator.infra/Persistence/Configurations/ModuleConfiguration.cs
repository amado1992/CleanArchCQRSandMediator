using CleanArchCQRSandMediator.Domain.Entities.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            // builder.ToTable("modules"); // Specify the exact name of the table in the database

            // Required one-to-many
            builder.HasMany(x => x.Permissions)
                   .WithOne(x => x.Module)
                   .HasForeignKey(x => x.ModuleId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
