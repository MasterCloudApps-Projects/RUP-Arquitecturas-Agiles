using Microsoft.EntityFrameworkCore;
using ShareThings.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace ShareThings.Data.Context
{
    public interface IShareThingsDbContext
    {
        DbSet<Borrow> Borrows { get; set; }
        DbSet<BorrowComment> Comments { get; set; }
        DbSet<Photo> Photos { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<BorrowScore> Scores { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}