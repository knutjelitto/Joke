using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class ThisLiteral : Expression
    {
        public ThisLiteral(TSpan span)
            : base(span)
        {
        }
    }
}
