using CleanArchCQRSandMediator.Domain.Entities.Business.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchCQRSandMediator.infra.Persistence.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            // Optional one-to-many
            builder.HasMany(x => x.Products)
                   .WithOne(x => x.Brand)
                   .HasForeignKey(x => x.BrandId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Tenant)
                   .WithMany(e => e.Brands)
                   .HasForeignKey(e => e.TenantId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
