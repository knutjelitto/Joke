using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class BinaryOp : Expression
    {
        public BinaryOp(TSpan span, BinaryOpKind kind, Expression left, Expression right)
            : base(span)
        {
            Kind = kind;
            Left = left;
            Right = right;
        }

        public BinaryOpKind Kind { get; }
        public Expression Left { get; }
        public Expression Right { get; }
    }
}
