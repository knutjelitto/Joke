using System;

namespace Joke.Front.Pony.Err
{
    public class JokeException : Exception
    {
        public JokeException(Error error)
        {
            Error = error;
        }
        public Error Error { get; }
    }
}
