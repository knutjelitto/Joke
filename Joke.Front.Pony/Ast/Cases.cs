using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Cases : Node
    {
        public Cases(TokenSpan span, IReadOnlyList<Case> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Case> Items { get; }
    }
}
