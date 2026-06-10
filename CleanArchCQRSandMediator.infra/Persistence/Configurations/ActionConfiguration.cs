using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class ActionConfiguration : IEntityTypeConfiguration<Domain.Entities.Business.Action>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Business.Action> builder)
        {
            // builder.ToTable("actions"); // Specify the exact name of the table in the database

            // Required one-to-many
            builder.HasMany(x => x.Permissions)
                   .WithOne(x => x.Action)
                   .HasForeignKey(x => x.ActionId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
