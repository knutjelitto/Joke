using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public abstract class Node
    {
        protected Node(TSpan span)
        {
            Span = span;
        }

        public TSpan Span { get; }
    }
}
