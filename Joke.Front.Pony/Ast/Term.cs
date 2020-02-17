using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Term : Expression
    {
        public Term(TSpan span)
            : base(span)
        {
        }
    }
}
