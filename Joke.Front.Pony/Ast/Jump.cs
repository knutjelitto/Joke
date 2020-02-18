using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Jump : Expression
    {
        public Jump(TSpan span, JumpKind kind, Expression? value)
            : base(span)
        {
            Kind = kind;
            Value = value;
        }

        public JumpKind Kind { get; }
        public Expression? Value { get; }
    }
}
