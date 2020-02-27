using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtUnary : PtExpression
    {
        public PtUnary(PonyTokenSpan span, PtUnaryKind kind, PtExpression expression)
            : base(span)
        {
            Kind = kind;
            Expression = expression;
        }

        public PtUnaryKind Kind { get; }
        public PtExpression Expression { get; }
    }
}
