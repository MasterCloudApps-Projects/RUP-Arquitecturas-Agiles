using FluentAssertions;
using ShareThings.Domain;
using ShareThings.Domain.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace ShareThings.UnitTest.Domain
{
    public class BorrowDomainTest
    {
        [Fact]
        public void GivenBorrowWhenBorrowerIsSameOwnerProductThenThrowBorrowerIsSameOwnerProductException()
        {
            User borrower = new User("Test");
            Product product = new Product(borrower, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            Assert.Throws<BorrowerIsSameOwnerProductException>(() =>
                new Borrow(borrower, product, new Term(DateTime.Now.AddDays(-6), DateTime.Now))
            );
        }

        [Fact]
        public void GivenBorrowWhenProductIsNotSharyThenThrowStatusInvalidException()
        {
            User borrower = new User("Test");
            User lender = new User("Test1");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            product.UnShary();
            Assert.Throws<StatusInvalidException>(() =>
                new Borrow(borrower, product, new Term(DateTime.Now.AddDays(-6), DateTime.Now))
            );
        }

        [Fact]
        public void GivenBorrowWhenProductIsNotAvailableThenThrowProductNoAvailableException()
        {
            User borrower = new User("Test");
            User lender = new User("Test1");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            Assert.Throws<ProductNoAvailableException>(() =>
                new Borrow(borrower, product, new Term(DateTime.Now.AddDays(-6), DateTime.Now))
            );
        }

        [Fact]
        public void GivenBorrowWhenBorrowConfirmThenProductBorrowed()
        {
            User borrower = new User("Test");
            User lender = new User("Test1");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            Borrow borrow = new Borrow(borrower, product, new Term(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1)));
            product.Status.Should().Be(ProductStatus.Shary);
            borrow.Status.Should().Be(BorrowStatus.Created);
            borrow.IsLender(lender).Should().BeTrue();
            borrow.IsBorrower(borrower).Should().BeTrue();
            borrow.Confirm();
            product.Status.Should().Be(ProductStatus.Borrowed);
            borrow.Status.Should().Be(BorrowStatus.Accepted);
        }

        [Fact]
        public void GivenBorrowWhenBorrowClosedThenProductShary()
        {
            User borrower = new User("Test");
            User lender = new User("Test1");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            Borrow borrow = new Borrow(borrower, product, new Term(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1)));
            product.Status.Should().Be(ProductStatus.Shary);
            borrow.Status.Should().Be(BorrowStatus.Created);
            borrow.Confirm();
            product.Status.Should().Be(ProductStatus.Borrowed);
            borrow.Status.Should().Be(BorrowStatus.Accepted);
            borrow.Close();
            product.Status.Should().Be(ProductStatus.Shary);
            borrow.Status.Should().Be(BorrowStatus.Closed);
        }

        [Fact]
        public void GivenBorrowWhenAddCommentThenCommentIsAdded()
        {
            User borrower = new User("Test");
            User lender = new User("Test1");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            Borrow borrow = new Borrow(borrower, product, new Term(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1)));
            Assert.False(borrow.Comments.Any());
            borrow.AddComment("comment", lender);
            Assert.True(borrow.Comments.Any());
        }

        [Fact]
        public void GivenBorrowWhenAddCommentInBlankThenCommentNotIsAddedAndThrowException()
        {
            User borrower = new User("Test");
            User lender = new User("Test1");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            Borrow borrow = new Borrow(borrower, product, new Term(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1)));
            Assert.False(borrow.Comments.Any());
            Assert.Throws<ArgumentException>(() => borrow.AddComment("", lender));
            Assert.False(borrow.Comments.Any());
        }

        [Fact]
        public void GivenBorrowWhenAddScoreThenScoreIsAddedAndAverageIsCalculateOk()
        {
            int score1 = 5;
            int score2 = 1;
            int average = (score1 + score2) / 2;
            User borrower = new User("Test");
            User lender = new User("Test1");
            Product product = new Product(lender, "Test", "Test", new ProductType(), new Term(DateTime.Now.AddDays(-6), DateTime.Now));
            Borrow borrow = new Borrow(borrower, product, new Term(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1)));
            Assert.False(borrow.Scores.Any());
            borrow.AddScore(new Score(score1), lender);
            borrow.AddScore(new Score(score2), borrower);
            Assert.True(borrow.Scores.Any());
            borrow.AverageScore().Should().Be(average);
        }
    }
}