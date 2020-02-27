using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtJump : PtExpression
    {
        public PtJump(PonyTokenSpan span, PtJumpKind kind, PtExpression? value)
            : base(span)
        {
            Kind = kind;
            Value = value;
        }

        public PtJumpKind Kind { get; }
        public PtExpression? Value { get; }
    }
}
