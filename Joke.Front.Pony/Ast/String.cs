using Joke.Front.Pony.Lex;
using System;

namespace Joke.Front.Pony.Ast
{
    public class String : Expression
    {
        public String(TSpan span)
            : base(span)
        {
        }
    }
}
