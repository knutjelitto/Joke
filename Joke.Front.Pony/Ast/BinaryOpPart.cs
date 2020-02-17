using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class BinaryOpPart : InfixPart
    {
        public BinaryOpPart(TSpan span, BinaryKind kind, Expression right)
            : base(span, kind)
        {
            Right = right;
        }

        public Expression Right { get; }
    }
}
