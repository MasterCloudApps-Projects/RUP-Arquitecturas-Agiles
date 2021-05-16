using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShareThings.Domain;

namespace ShareThings.Data.Configurations
{
    public sealed class CommentConfiguration : IEntityTypeConfiguration<BorrowComment>
    {
        public void Configure(EntityTypeBuilder<BorrowComment> builder)
        {
            builder.ToTable("Comment");
            builder.HasKey(e => new { e.CommentId });
            builder.Property(e => e.Text)
                .HasColumnName("Text")
                .HasMaxLength(500);

            builder
                .HasOne(a => a.Owner)
                .WithMany(b => b.Comments)
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Comments_User");

            builder
                .HasOne(a => a.Borrow)
                .WithMany(b => b.Comments)
                .HasForeignKey(b => b.BorrowId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Comments_Borrow");
        }
    }
}
