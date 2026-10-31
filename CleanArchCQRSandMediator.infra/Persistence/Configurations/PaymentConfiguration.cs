using CleanArchCQRSandMediator.Domain.Entities.Business.Plan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount).HasPrecision(18, 2);
            builder.Property(x => x.Currency).HasMaxLength(3);
            builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.ExternalPaymentId).HasMaxLength(100);

            builder.HasOne(x => x.Subscription)
                .WithMany(s => s.Payments)
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Cascade); // If the subscription is deleted, its payments are deleted.

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
