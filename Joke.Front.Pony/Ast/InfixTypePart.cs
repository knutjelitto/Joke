using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class InfixTypePart : Node
    {
        public InfixTypePart(TSpan span, InfixTypeKind kind, Type right)
            : base(span)
        {
            Kind = kind;
            Right = right;
        }

        public InfixTypeKind Kind { get; }
        public Type Right { get; }
    }
}
