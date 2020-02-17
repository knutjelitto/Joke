using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Fields : Node
    {
        public Fields(TSpan span, IReadOnlyList<Field> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Field> Items { get; }
    }
}
