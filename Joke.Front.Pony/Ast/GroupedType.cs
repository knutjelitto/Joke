using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class GroupedType : Type
    {
        public GroupedType(TokenSpan span, IReadOnlyList<Type> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Type> Items { get; }
    }
}
