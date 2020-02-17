using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Char : Expression
    {
        public Char(TSpan span)
            : base(span)
        {
        }
    }
}
