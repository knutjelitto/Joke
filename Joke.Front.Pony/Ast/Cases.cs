using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Cases : Node
    {
        public Cases(PonyTokenSpan span, IReadOnlyList<Case> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Case> Items { get; }
    }
}
