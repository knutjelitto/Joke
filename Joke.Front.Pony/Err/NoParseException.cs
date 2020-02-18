using System;

namespace Joke.Front.Pony
{
    public class NoParseException : Exception
    {
        public NoParseException(string? message)
            : base(message)
        {
        }
    }
}
