using System;

namespace ShareThings.Domain.Exceptions
{
    public class ProductNoAvailableException : Exception
    {
        public ProductNoAvailableException(string message)
            : base(message)
        { }
    }
}