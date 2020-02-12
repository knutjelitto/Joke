using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public abstract class Node
    {
        private readonly TSpan span;

        protected Node(TSpan span)
        {
            this.span = span;
        }
    }
}
