using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Cap : Node
    {
        public Cap(TSpan span, CapKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public CapKind Kind { get; }
    }
}
