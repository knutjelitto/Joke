using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Consume : Expression
    {
        public Consume(ISpan span, Capability? caps, Expression term)
            : base(span)
        {
            Caps = caps;
            Term = term;
        }

        public Capability? Caps { get; }
        public Expression Term { get; }
    }
}
