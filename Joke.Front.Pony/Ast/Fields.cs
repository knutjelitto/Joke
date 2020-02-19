using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Fields : Node
    {
        public Fields(TokenSpan span, IReadOnlyList<Field> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Field> Items { get; }
    }
}
