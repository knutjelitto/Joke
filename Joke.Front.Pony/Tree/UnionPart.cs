using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class UnionPart : InfixTypePart
    {
        public UnionPart(TSpan span, Type right)
            : base(span)
        {
            Right = right;
        }

        public Type Right { get; }
    }
}
