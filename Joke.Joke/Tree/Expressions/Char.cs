using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Char : ILiteral
    {
        public Char(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
