using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class GroupedType : Type
    {
        public GroupedType(TSpan span, IReadOnlyList<Type> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Type> Items { get; }
    }
}
