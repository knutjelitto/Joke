using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Jump : Expression
    {
        public Jump(PonyTokenSpan span, JumpKind kind, Expression? value)
            : base(span)
        {
            Kind = kind;
            Value = value;
        }

        public JumpKind Kind { get; }
        public Expression? Value { get; }
    }
}
