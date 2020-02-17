using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Then : Expression
    {
        public Then(TSpan span, Annotations? annotations, Expression body)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
        }

        public Annotations? Annotations { get; }
        public Expression Body { get; }
    }
}
