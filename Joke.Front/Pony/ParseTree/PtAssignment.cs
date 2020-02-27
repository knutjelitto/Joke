using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtAssignment : PtExpression
    {
        public PtAssignment(PonyTokenSpan span, PtExpression left, PtExpression right)
            : base(span)
        {
            Left = left;
            Right = right;
        }

        public PtExpression Left { get; }
        public PtExpression Right { get; }
    }
}
