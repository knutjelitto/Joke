using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Dot : Postfix
    {
        public Dot(ISpan span, Expression postfixed, Identifier member)
            : base(span, postfixed)
        {
            Member = member;
        }

        public Identifier Member { get; }
    }
}
