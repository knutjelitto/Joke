using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Int : Expression
    {
        public Int(TSpan span)
            : base(span)
        {
        }
    }
}
