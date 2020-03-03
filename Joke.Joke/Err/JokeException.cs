using System;

namespace Joke.Joke.Err
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
