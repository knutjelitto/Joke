using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class DotIdentifier : Identifier
    {
        public DotIdentifier(TSpan span, Identifier before, Identifier after)
            : base(span)
        {
            Before = before;
            After = after;
        }

        public Identifier Before { get; }
        public Identifier After { get; }
    }
}
