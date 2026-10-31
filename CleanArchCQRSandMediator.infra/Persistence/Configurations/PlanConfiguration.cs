using CleanArchCQRSandMediator.Domain.Entities.Business.Plan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable("Plans");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Price).HasPrecision(18, 2);
            builder.Property(x => x.BillingPeriod).HasConversion<string>().HasMaxLength(50);

            // many-to-many
            builder.HasMany(e => e.Features)
                   .WithMany(e => e.Plans)
                   .UsingEntity<PlanFeature>(
                       r => r.HasOne<Feature>(e => e.Feature).WithMany(e => e.PlanFeatures).HasForeignKey(e => e.FeatureId).HasPrincipalKey(e => e.Id).OnDelete(DeleteBehavior.Restrict),
                       l => l.HasOne<Plan>(e => e.Plan).WithMany(e => e.PlanFeatures).HasForeignKey(e => e.PlanId).HasPrincipalKey(e => e.Id).OnDelete(DeleteBehavior.Restrict),
                       j => 
                       {
                           j.HasKey(pf => new { pf.PlanId, pf.FeatureId });
                           j.ToTable("PlanFeatures"); 
                       });

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
