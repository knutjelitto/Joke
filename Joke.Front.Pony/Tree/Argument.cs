using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public abstract class Argument : Expression
    {
        public Argument(TSpan span)
            : base(span)
        {
        }
    }
}
