using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShareThings.Domain
{
    public interface IBorrowRepository : IRepository<Borrow>
    {
        Task<List<Borrow>> GetAllByLender(User lender);
        Task<List<Borrow>> GetAllByBorrower(User borrower);
    }
}