using CleanArchCQRSandMediator.Domain.Entities.Business.Plan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class FeatureConfiguration : IEntityTypeConfiguration<Feature>
    {
        public void Configure(EntityTypeBuilder<Feature> builder)
        {
            builder.ToTable("Features");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.Code).IsUnique(); // Unique Key

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(50);
        }
    }
}
