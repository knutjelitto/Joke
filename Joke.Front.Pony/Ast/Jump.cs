using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Jump : Expression
    {
        public Jump(ISpan span, JumpKind kind, Expression? value)
            : base(span)
        {
            Kind = kind;
            Value = value;
        }

        public JumpKind Kind { get; }
        public Expression? Value { get; }
    }
}
