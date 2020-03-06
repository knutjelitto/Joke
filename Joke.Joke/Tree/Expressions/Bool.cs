using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Bool : IExpression
    {
        public Bool(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
