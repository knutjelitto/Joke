using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ThisValue : IExpression
    {
        public ThisValue(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
