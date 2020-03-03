using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Identifier
    {
        public Identifier(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
