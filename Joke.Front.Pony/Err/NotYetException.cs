using System;

namespace Joke.Front.Pony
{
    public class NotYetException : Exception
    {
        public NotYetException(string message)
            : base("not implemented: " + message)
        {
        }
    }
}
