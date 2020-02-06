using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Infix : Expression
    {
        public Infix(ISpan span, InfixOp op, Expression left, Expression right)
            : base(span)
        {
        }
    }
}
