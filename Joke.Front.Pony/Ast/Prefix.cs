using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Prefix : Expression
    {
        public Prefix(ISpan span, PrefixOp op, Expression expression)
            : base(span)
        {
            Op = op;
            Expression = expression;
        }

        public PrefixOp Op { get; }
        public Expression Expression { get; }
    }
}
