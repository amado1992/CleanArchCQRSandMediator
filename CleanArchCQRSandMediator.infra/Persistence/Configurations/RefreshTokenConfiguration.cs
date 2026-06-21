using CleanArchCQRSandMediator.Domain.Entities.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Required one-to-many, beginning with the dependent entity type (RefreshToken)
            builder.HasOne(e => e.ApplicationUser)
                   .WithMany(e => e.RefreshTokens)
                   .HasForeignKey(e => e.ApplicationUserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasKey(rt => rt.Id); // (Optional), it already does so by convention
            builder.HasIndex(e => e.Token).IsUnique();
        }
    }
}
