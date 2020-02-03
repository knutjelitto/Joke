using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Joke.Ast
{
    public class Item
    {
        public Item(SourceSpan span)
        {
            Span = span;
        }

        public SourceSpan Span { get; }
    }
}
