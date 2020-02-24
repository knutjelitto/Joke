using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Fields : Node
    {
        public Fields(PonyTokenSpan span, IReadOnlyList<Field> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Field> Items { get; }
    }
}
