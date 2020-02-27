using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtSemiExpression : PtExpression
    {
        public PtSemiExpression(PonyTokenSpan span, PtExpression expression)
            : base(span)
        {
            Expression = expression;
        }

        public PtExpression Expression { get; }
    }
}
