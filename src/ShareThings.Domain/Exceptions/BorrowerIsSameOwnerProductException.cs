using System;

namespace ShareThings.Domain.Exceptions
{
    public class BorrowerIsSameOwnerProductException : Exception
    {
        public BorrowerIsSameOwnerProductException(string message)
            : base(message)
        { }
    }
}