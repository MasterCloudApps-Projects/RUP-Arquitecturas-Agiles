using System;

namespace ShareThings.Domain.Exceptions
{
    public class StatusInvalidException : Exception
    {
        public StatusInvalidException(string message)
            : base(message)
        { }
    }
}