using CleanArchCQRSandMediator.Domain.Entities.Business.Plan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class UsageRecordConfiguration : IEntityTypeConfiguration<UsageRecord>
    {
        public void Configure(EntityTypeBuilder<UsageRecord> builder)
        {
            builder.ToTable("UsageRecords");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Subscription)
                .WithMany(s => s.UsageRecords)
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Feature)
                .WithMany(f => f.UsageRecords)
                .HasForeignKey(x => x.FeatureId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for quick limit lookups (SUM(Quantity) WHERE SubId AND FeatureId)
            builder.HasIndex(x => new { x.SubscriptionId, x.FeatureId });

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
