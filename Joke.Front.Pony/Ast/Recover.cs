using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Recover : Expression
    {
        public Recover(ISpan span, Capability? caps, Expression body)
            : base(span)
        {
            Caps = caps;
            Body = body;
        }

        public Capability? Caps { get; }
        public Expression Body { get; }
    }
}
