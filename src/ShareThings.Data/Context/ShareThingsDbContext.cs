using Microsoft.EntityFrameworkCore;
using ShareThings.Domain;

namespace ShareThings.Data.Context
{
    public sealed class ShareThingsDbContext : DbContext, IShareThingsDbContext
    {
        #region Constructor
        public ShareThingsDbContext(DbContextOptions<ShareThingsDbContext> options)
            : base(options) { }
        #endregion

        #region Properties
        public DbSet<BorrowScore> Scores { get; set; }
        public DbSet<BorrowComment> Comments { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShareThingsDbContext).Assembly);
        }
        #endregion
    }
}
