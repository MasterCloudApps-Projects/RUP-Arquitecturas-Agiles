using ShareThings.Domain;
using ShareThings.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareThings.UnitTest
{
    public static class FakeBuilder
    {
        private static readonly Random _random = new Random();

        public static BorrowViewModel GetBorrowViewModelFake(int borrowId, int productId)
        {
            return new BorrowViewModel
            {
                BorrowId = borrowId,
                BorrowerId = GetBorrowerIdFake(),
                BorrowerName = GetStringFake(50),
                ProductOwnerId = GetOwnerIdFake(),
                ProductOwnerUserIdentity = Guid.NewGuid().ToString(),
                ProductOwner = GetStringFake(50),
                ProductId = productId,
                ProductName = GetStringFake(50),
                Status = BorrowStatus.Created.ToString(),
                Start = DateTime.Now.AddDays(5),
                End = DateTime.Now.AddMonths(5),
            };
        }

        public static List<Borrow> GetBorrowsFake()
        {
            return new List<Borrow>
            {
                GetBorrowFake(),
                GetBorrowFake(),
                GetBorrowFake(),
                GetBorrowFake()
            };
        }

        public static Borrow GetBorrowFake()
        {
            return new Borrow(
                GetBorrowerFake(),
                GetProductFake(),
                new Term(DateTime.Now, DateTime.Now.AddDays(12))
            );
        }

        public static ProductViewModel GetProductViewModelFake(int productId)
        {
            return new ProductViewModel
            {
                ProductId = productId,
                Name = GetStringFake(15),
                Description = GetStringFake(50),
                Status = ProductStatus.Shary.ToString(),
                Type = GetStringFake(4),
                Subtype = GetStringFake(4),
                Start = DateTime.Now.AddDays(5),
                End = DateTime.Now.AddMonths(5)
            };
        }

        public static Product GetProductFake()
        {
            return new Product(
                GetOwnerFake(),
                GetStringFake(15), 
                GetStringFake(50), 
                new ProductType(GetStringFake(4), GetStringFake(4)),
                new Term(DateTime.Now, DateTime.Now.AddYears(1))
            );
        }

        public static Product GetProductFake(User owner)
        {
            return new Product(
                owner,
                GetStringFake(15),
                GetStringFake(50),
                new ProductType(GetStringFake(4), GetStringFake(4)),
                new Term(DateTime.Now, DateTime.Now.AddYears(1))
            );
        }

        public static List<Product> GetProductsFake()
        {
            return new List<Product>
            {
                GetProductFake(),
                GetProductFake(),
                GetProductFake(),
                GetProductFake()
            };
        }

        public static User GetOwnerFake()
        {
            return new User(
                GetOwnerIdFake(),
                Guid.NewGuid().ToString());
        }

        public static User GetBorrowerFake()
        {
            return new User(
                GetBorrowerIdFake(),
                Guid.NewGuid().ToString());
        }

        private static int GetBorrowerIdFake()
        {
            return GetNumberFake(1001, 2000);
        }

        private static int GetOwnerIdFake()
        {
            return GetNumberFake(1, 1000);
        }

        private static string GetStringFake(int size, bool lowerCase = false)
        {
            StringBuilder builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                char @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        private static int GetNumberFake(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
