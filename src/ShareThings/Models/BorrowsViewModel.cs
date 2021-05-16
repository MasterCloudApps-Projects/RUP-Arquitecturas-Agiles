using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareThings.Models
{
    public class BorrowsViewModel
    {
        public List<BorrowViewModel> Lender { get; set; }
        public List<BorrowViewModel> Borrower { get; set; }
    }
}
