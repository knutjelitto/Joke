using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Term : Expression
    {
        public Term(TSpan span)
            : base(span)
        {
        }
    }
}
