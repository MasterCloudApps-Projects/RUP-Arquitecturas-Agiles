using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShareThings.Domain;

namespace ShareThings.Data.Configurations
{
    public sealed class ScoreConfiguration : IEntityTypeConfiguration<BorrowScore>
    {
        public void Configure(EntityTypeBuilder<BorrowScore> builder)
        {
            builder.ToTable("Score");
            builder.HasKey(e => new { e.ScoreId });
            builder.OwnsOne(p => p.Punctuation);

            builder
                .HasOne(a => a.Owner)
                .WithMany(b => b.Scores)
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Scores_User");

            builder
                .HasOne(a => a.Borrow)
                .WithMany(b => b.Scores)
                .HasForeignKey(b => b.BorrowId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Scores_Borrow");
        }
    }
}