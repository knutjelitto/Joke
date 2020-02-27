using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtConsume : PtExpression
    {
        public PtConsume(PonyTokenSpan span, PtCap? cap, PtExpression expression)
            : base(span)
        {
            Cap = cap;
            Expression = expression;
        }

        public PtCap? Cap { get; }
        public PtExpression Expression { get; }
    }
}
