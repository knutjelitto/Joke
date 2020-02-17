using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Location : Expression
    {
        public Location(TSpan span)
            : base(span)
        {
        }
    }
}
