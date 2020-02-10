using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Chain : Postfix
    {
        public Chain(ISpan span, Expression postfixed, Identifier member)
            : base(span, postfixed)
        {
            Member = member;
        }

        public Identifier Member { get; }
    }
}
