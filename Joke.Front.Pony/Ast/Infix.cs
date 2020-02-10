using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Infix : Expression
    {
        public Infix(ISpan span, InfixOp op, Boolean @unsafe, Expression left, Expression right)
            : base(span)
        {
            Unsafe = @unsafe;
            Left = left;
            Right = right;
        }

        public Boolean Unsafe { get; }
        public Expression Left { get; }
        public Expression Right { get; }
    }
}
