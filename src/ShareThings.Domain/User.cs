using System;
using System.Collections.Generic;

namespace ShareThings.Domain
{
    public sealed class User
    {
        private readonly List<BorrowScore> _scores;
        private readonly List<Product> _products;
        private readonly List<BorrowComment> _comments;
        private readonly List<Borrow> _borrows;

        private User()
        {
            this._scores = new List<BorrowScore>();
            this._products = new List<Product>();
            this._comments = new List<BorrowComment>();
            this._borrows = new List<Borrow>();
        }

        public User(string userIdentityId) : this()
        {
            if (string.IsNullOrWhiteSpace(userIdentityId))
                throw new ArgumentException("The userIdentityId is not valid", nameof(userIdentityId));

            this.UserIdentityId = userIdentityId;
        }

        public User(int userId, string userIdentityId) : this(userIdentityId)
        {
            if (userId < 0)
                throw new ArgumentException("The userId is not valid", nameof(userId));

            this.UserId = userId;
        }

        public int UserId { get; private set; }
        public string UserIdentityId { get; private set; }

        public IReadOnlyCollection<BorrowScore> Scores => _scores;
        public IReadOnlyCollection<Product> Products => _products;
        public IReadOnlyCollection<BorrowComment> Comments => _comments;
        public IReadOnlyCollection<Borrow> Borrows => _borrows;

        public void UnRegister()
        {
            this.UserIdentityId = string.Empty;
        }
    }
}