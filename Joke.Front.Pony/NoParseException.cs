using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony
{
    public class NoParseException : Exception
    {
        public NoParseException(string? message)
            : base("NO " + message)
        {
        }
    }
}
