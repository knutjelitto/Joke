using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class String : ILiteral
    {
        public String(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
