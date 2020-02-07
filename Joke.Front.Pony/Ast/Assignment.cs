using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Assignment : Expression
    {
        public Assignment(ISpan span, Expression lhs, Expression rhs)
            : base(span)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public Expression Lhs { get; }
        public Expression Rhs { get; }
    }
}
