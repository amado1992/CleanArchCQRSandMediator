using CleanArchCQRSandMediator.Domain.Entities.Business.Plan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).HasMaxLength(450).IsRequired(); // Default size for Identity
            builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.ExternalSubscriptionId).HasMaxLength(100);

            // GOLDEN RULE: A user can only have one active subscription at a time.
            builder.HasIndex(x => x.UserId)
                   .HasFilter(@"""Status"" = 'Active'") // Syntax for PostgreSQL
                   .IsUnique();

            builder.HasOne(x => x.Plan)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Optional one-to-many
            builder.HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.UpdatedBy)
                .WithMany()
                .HasForeignKey(t => t.UpdatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
