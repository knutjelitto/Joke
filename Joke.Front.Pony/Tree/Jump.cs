using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
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
