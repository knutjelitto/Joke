using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Cap : Type
    {
        public Cap(TSpan span, CapKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public CapKind Kind { get; }
    }
}
