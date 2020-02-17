using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public abstract class Ids : Node
    {
        protected Ids(TSpan span) : base(span)
        {
        }
    }
}
