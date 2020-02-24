using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class GroupedType : Type
    {
        public GroupedType(PonyTokenSpan span, IReadOnlyList<Type> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Type> Items { get; }
    }
}
