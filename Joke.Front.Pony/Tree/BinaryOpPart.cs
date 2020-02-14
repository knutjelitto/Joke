using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class BinaryOpPart : InfixPart
    {
        public BinaryOpPart(TSpan span, BinaryOpKind kind, Partial? partial, Expression right)
            : base(span)
        {
            Kind = kind;
            Partial = partial;
            Right = right;
        }

        public BinaryOpKind Kind { get; }
        public Partial? Partial { get; }
        public Expression Right { get; }
    }
}
