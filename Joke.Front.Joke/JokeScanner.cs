using Joke.Front.Err;

namespace Joke.Front.Joke
{
    public class JokeScanner
    {
        public JokeScanner(IErrors errors, ISource source)
        {
            Errors = errors;
            Source = source;
            Content = Source.Content;
        }

        public IErrors Errors { get; }
        public ISource Source { get; }
        public string Content { get; }
    }
}
