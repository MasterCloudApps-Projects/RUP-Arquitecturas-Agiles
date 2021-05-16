using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShareThings.Domain;

namespace ShareThings.Data.Configurations
{
    public sealed class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.ToTable("Photo");
            builder.HasKey(e => new { e.PhotoId });

            builder
                .HasOne(a => a.Product)
                .WithMany(b => b.Photos)
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Photos_Product");
        }
    }
}
