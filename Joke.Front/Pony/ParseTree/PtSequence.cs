using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtSequence : PtExpression
    {
        public PtSequence(PonyTokenSpan span, PtExpression first, PtExpression next)
            : base(span)
        {
            First = first;
            Next = next;
        }

        public PtExpression First { get; }
        public PtExpression Next { get; }
    }
}
