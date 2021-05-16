using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShareThings.Domain;

namespace ShareThings.Data.Configurations
{
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(e => new { e.ProductId });
            builder.Property(e => e.Description)
                .HasColumnName("Description")
                .HasMaxLength(250);
            builder.OwnsOne(p => p.Family);
            builder.OwnsOne(p => p.Availability);
            builder
                .HasOne(a => a.Owner)
                .WithMany(b => b.Products)
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Products_User");
        }
    }
}
