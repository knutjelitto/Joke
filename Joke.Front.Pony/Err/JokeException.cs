using System;

namespace Joke.Front.Pony.Err
{
    public class JokeException : Exception
    {
        public JokeException(IDescription description)
        {
            Description = description;
        }

        public IDescription Description { get; }
    }
}
