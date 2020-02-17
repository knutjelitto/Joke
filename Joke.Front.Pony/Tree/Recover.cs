using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Recover : Expression
    {
        public Recover(TSpan span, Annotations? annotations, Cap? cap, Expression body) : base(span)
        {
            Annotations = annotations;
            Cap = cap;
            Body = body;
        }

        public Annotations? Annotations { get; }
        public Cap? Cap { get; }
        public Expression Body { get; }
    }
}
