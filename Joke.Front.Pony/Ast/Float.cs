using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Float : Expression
    {
        public Float(TSpan span)
            : base(span)
        {
        }
    }
}
