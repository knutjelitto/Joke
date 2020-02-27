using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtGuard : PtExpression
    {
        public PtGuard(PonyTokenSpan span, PtExpression expression)
            : base(span)
        {
            Expression = expression;
        }

        public PtExpression Expression { get; }
    }
}
