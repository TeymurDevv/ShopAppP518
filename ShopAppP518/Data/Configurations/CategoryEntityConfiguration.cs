using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopAppP518.Entities;

namespace ShopAppP518.Data.Configurations
{
    public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Name).IsRequired(true).HasMaxLength(50);
            builder.HasIndex(p => p.Name).IsUnique();
            builder.Property(p=>p.IsDelete).HasDefaultValue(false);
            builder.Property(p=>p.CreatedDate).HasDefaultValueSql("getdate()");
            builder.Property(p=>p.UpdatedDate).HasDefaultValueSql("getdate()");
        }
    }
}
