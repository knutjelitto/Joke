using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class AsPart : InfixPart
    {
        public AsPart(TSpan span, Type type)
            : base(span)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
