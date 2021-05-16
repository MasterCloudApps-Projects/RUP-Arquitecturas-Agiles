using System;

namespace ShareThings.Domain.Exceptions
{
    public class PhotoNotFoundException : Exception
    {
        public PhotoNotFoundException(string message)
            : base(message)
        { }
    }
}