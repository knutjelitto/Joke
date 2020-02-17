using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Partial : Node
    {
        public Partial(TSpan span)
            : base(span)
        {
        }
    }
}
