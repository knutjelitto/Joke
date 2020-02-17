using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class Expression : Node
    {
        protected Expression(TSpan span)
            : base(span)
        {
        }
    }
}
