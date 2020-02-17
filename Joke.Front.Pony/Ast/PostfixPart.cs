using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class PostfixPart : Expression
    {
        public PostfixPart(TSpan span) : base(span)
        {
        }
    }
}
