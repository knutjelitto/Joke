using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Continue : IExpression
    {
        public Continue(TokenSpan span, IExpression? value)
        {
            Span = span;
            Value = value;
        }

        public TokenSpan Span { get; }
        public IExpression? Value { get; }
    }
}
