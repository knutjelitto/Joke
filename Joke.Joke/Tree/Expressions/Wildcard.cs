using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Wildcard : IExpression
    {
        public Wildcard(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
