using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class EphemAlias : Node
    {
        public EphemAlias(TSpan span, EAKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public EAKind Kind { get; }
    }
}
