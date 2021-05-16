using System;

namespace ShareThings.Domain.Exceptions
{
    public class ScoreOutRangeException : Exception
    {
        public ScoreOutRangeException(string message)
            : base(message)
        { }
    }
}