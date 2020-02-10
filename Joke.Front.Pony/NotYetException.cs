using System;
using System.Collections.Generic;
using System.Text;

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
