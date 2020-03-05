using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Integer : ILiteral
    {
        public Integer(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
