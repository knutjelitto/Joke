using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Parameter : Node
    {
        public Parameter(TSpan span)
            : base(span)
        {
        }
    }
}
