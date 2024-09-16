using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopAppP518.Entities;

namespace ShopAppP518.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.Property(s => s.IsMain).HasDefaultValue(false);
            builder.Property(s => s.UpdatedDate).HasDefaultValueSql("GETDATE()");
            builder.Property(s => s.CreatedDate).HasDefaultValueSql("GETDATE()");

        }
    }
}
