using System;

namespace ShareThings.Domain
{
    public sealed class BorrowScore
    {
        private BorrowScore()
        {
        }

        public BorrowScore(Score score, User owner, Borrow borrow) : this()
        {
            this.Punctuation = score ?? throw new ArgumentNullException(nameof(owner));
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.Borrow = borrow ?? throw new ArgumentNullException(nameof(borrow));
        }

        public int ScoreId { get; private set; }
        public Score Punctuation { get; private set; }
        public int OwnerId { get; private set; }
        public User Owner { get; private set; }
        public int BorrowId { get; private set; }
        public Borrow Borrow { get; private set; }

        public int GetScore()
        {
            return this.Punctuation.Punctuation;
        }
    }
}