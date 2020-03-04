using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Error : IExpression
    {
        public Error(TokenSpan span, IExpression? value)
        {
            Span = span;
            Value = value;
        }

        public TokenSpan Span { get; }
        public IExpression? Value { get; }
    }
}
