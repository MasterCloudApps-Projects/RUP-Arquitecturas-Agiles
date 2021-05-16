using Microsoft.EntityFrameworkCore;
using ShareThings.Data.Context;
using ShareThings.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareThings.Data.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly IShareThingsDbContext _context;

        public BorrowRepository(IShareThingsDbContext shareThingsDbContext)
        {
            this._context = shareThingsDbContext ?? throw new ArgumentNullException(nameof(shareThingsDbContext));
        }

        public async Task Save(Borrow entity)
        {
            if(entity.BorrowId > 0)
                this._context.Borrows.Update(entity);
            else
                this._context.Borrows.Add(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task<Borrow> Get(int id)
        {
            return await this._context.Borrows
                .Include(b => b.Product)
                .Include(b => b.Product.Owner)
                .Include(b => b.Borrower)
                .Include(b => b.Comments)
                .Include(b => b.Scores)
                .FirstOrDefaultAsync(b => b.BorrowId == id);
        }

        public async Task<List<Borrow>> GetAllByLender(User lender)
        {
            List<Borrow> list = await this._context.Borrows
                .Include(b => b.Product)
                .Include(b => b.Product.Owner)
                .Include(b => b.Borrower)
                .Include(b => b.Scores)
                .ToListAsync();

            return list
                .Where(b => b.IsLender(lender))
                .ToList();
        }

        public async Task<List<Borrow>> GetAllByBorrower(User borrower)
        {
            List<Borrow> list = await this._context.Borrows
                .Include(b => b.Product)
                .Include(b => b.Product.Owner)
                .Include(b => b.Borrower)
                .Include(b => b.Scores)
                .ToListAsync();

            return list
                .Where(b => b.IsBorrower(borrower))
                .ToList();
        }
    }
}