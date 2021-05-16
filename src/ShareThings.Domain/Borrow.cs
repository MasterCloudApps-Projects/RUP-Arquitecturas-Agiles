using ShareThings.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShareThings.Domain
{
    public sealed class Borrow
    {
        private readonly List<BorrowComment> _comments;
        private readonly List<BorrowScore> _scores;

        private Borrow()
        {
            this._comments = new List<BorrowComment>();
            this._scores = new List<BorrowScore>();
        }

        public Borrow(User borrower, Product product, Term duration) : this()
        {
            this.Borrower = borrower ?? throw new ArgumentNullException(nameof(borrower));
            this.BorrowerId = this.Borrower.UserId;
            this.Product = product ?? throw new ArgumentNullException(nameof(product));
            this.ProductId = this.Product.ProductId;
            if(this.Product.IsOwner(borrower))
                throw new BorrowerIsSameOwnerProductException("The borrower can't be the same as the owner of the product");
            if (!this.Product.IsShary())
                throw new StatusInvalidException("The product is not shary");
            if(!this.Product.IsAvailable(duration))
                throw new ProductNoAvailableException("The product is not available");
            this.Duration = duration ?? throw new ArgumentNullException(nameof(duration));
            this.Status = BorrowStatus.Created;
        }

        public int BorrowId { get; private set; }
        public int BorrowerId { get; private set; }
        public User Borrower { get; private set; }
        public User Lender { get { return this.Product.Owner; } }
        public int ProductId { get; private set; }
        public Product Product { get; private set; }
        public BorrowStatus Status { get; private set; }
        public Term Duration { get; private set; }
        public IReadOnlyCollection<BorrowScore> Scores => _scores;
        public IReadOnlyCollection<BorrowComment> Comments => _comments;

        public void EditDuration(Term newDuration)
        {
            this.Duration = newDuration ?? throw new ArgumentNullException(nameof(newDuration));
        }

        public void Confirm()
        {
            if (this.Status != BorrowStatus.Created)
                throw new StatusInvalidException($"In the current state {this.Status} it cannot be accepted");

            this.Status = BorrowStatus.Accepted;
            this.Product.Borrowed();
        }

        public void Reject()
        {
            if (this.Status != BorrowStatus.Created)
                throw new StatusInvalidException($"In the current state {this.Status} it cannot be accepted");

            this.Status = BorrowStatus.Rejected;
        }

        public void Close()
        {
            if (this.Status != BorrowStatus.Accepted)
                throw new StatusInvalidException($"In the current state {this.Status} it cannot be closed");

            this.Status = BorrowStatus.Closed;
            this.Product.Shary();
        }

        public bool IsBorrower(User user)
        {
            if (user == null)
                return false;

            return this.BorrowerId == user.UserId;
        }

        public bool IsLender(User user)
        {
            return this.Product.IsOwner(user);
        }

        public void AddComment(string text, User user)
        {
            BorrowComment comment = new BorrowComment(text, user, this);
            this._comments.Add(comment);
        }

        public void AddScore(Score score, User user)
        {
            BorrowScore borrowScore = new BorrowScore(score, user, this);
            this._scores.Add(borrowScore);
        }

        public int AverageScore()
        {
            if (!this.Scores.Any())
                return 0;

            return this.Scores.Sum(score => score.GetScore()) / this.Scores.Count;
        }
    }
}