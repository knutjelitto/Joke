using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLiteral<T> : PtExpression
    {
        public PtLiteral(PonyTokenSpan span, T value)
            : base(span)
        {
            Value = value;
        }

        public T Value { get; }

        public override string? ToString()
        {
            return Span.ToString();
        }
    }
}
