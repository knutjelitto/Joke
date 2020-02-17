using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Cases : Node
    {
        public Cases(TSpan span, IReadOnlyList<Case> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Case> Items { get; }
    }
}
