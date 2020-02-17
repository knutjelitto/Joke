using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Char : Expression
    {
        public Char(TSpan span)
            : base(span)
        {
        }
    }
}
