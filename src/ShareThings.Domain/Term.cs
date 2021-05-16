using System;

namespace ShareThings.Domain
{
    public class Term
    {
        public Term(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException($"{nameof(End)} needs to be greater than {nameof(Start)}.", nameof(start));
            }

            this.Start = start;
            this.End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public bool IsBetween(DateTime date)
        {
            return Start <= date && End >= date;
        }

        public bool IsTermInclude(Term term)
        {
            return IsBetween(term.Start) && IsBetween(term.End);
        }
    }
}