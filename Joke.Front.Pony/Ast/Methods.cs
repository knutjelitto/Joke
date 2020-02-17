using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Methods : Node
    {
        public Methods(TSpan span, IReadOnlyList<Method> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Method> Items { get; }
    }
}
