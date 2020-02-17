using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class AsPart : InfixPart
    {
        public AsPart(TSpan span, Type type)
            : base(span, BinaryKind.As)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
