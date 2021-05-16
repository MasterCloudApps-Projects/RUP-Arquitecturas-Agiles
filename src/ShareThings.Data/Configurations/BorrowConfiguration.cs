using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShareThings.Domain;

namespace ShareThings.Data.Configurations
{
    public sealed class BorrowConfiguration : IEntityTypeConfiguration<Borrow>
    {
        public void Configure(EntityTypeBuilder<Borrow> builder)
        {
            builder.ToTable("Borrow");
            builder.HasKey(e => new { e.BorrowId });
            builder.OwnsOne(p => p.Duration);
            builder
                .HasOne(a => a.Borrower)
                .WithMany(b => b.Borrows)
                .HasForeignKey(b => b.BorrowerId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Borrows_Borrower");
        }
    }
}
