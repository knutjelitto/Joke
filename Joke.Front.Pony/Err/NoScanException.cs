using System;

namespace Joke.Front.Pony
{
    public class NoScanException : Exception
    {
        public NoScanException(string? message)
            : base(message)
        {
        }
    }
}
