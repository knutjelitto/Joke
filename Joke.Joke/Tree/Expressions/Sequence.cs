using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Sequence : IExpression
    {
        public Sequence(TokenSpan span, IExpression first, IExpression next)
        {
            Span = span;
            First = first;
            Next = next;
        }

        public TokenSpan Span { get; }
        public IExpression First { get; }
        public IExpression Next { get; }
    }
}
