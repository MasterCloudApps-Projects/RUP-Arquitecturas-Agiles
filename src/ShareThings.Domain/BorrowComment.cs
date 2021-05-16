using System;

namespace ShareThings.Domain
{
    public sealed class BorrowComment
    {
        private BorrowComment()
        {
        }

        public BorrowComment(string text, User owner, Borrow borrow) : this()
        {
            if(string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Comment text cannot be empty", nameof(text));

            this.Text = text;
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.OwnerId = owner.UserId;
            this.Borrow = borrow ?? throw new ArgumentNullException(nameof(borrow));
            this.BorrowId = borrow.BorrowId;
            this.Creation = DateTime.Now;
        }

        public DateTime Creation { get; private set; }
        public int CommentId { get; private set; }
        public string Text { get; private set; }
        public int OwnerId { get; private set; }
        public User Owner { get; private set; }
        public int BorrowId { get; private set; }
        public Borrow Borrow { get; private set; }
    }
}