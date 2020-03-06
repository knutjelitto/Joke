using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Throws : IAny
    {
        public Throws(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
