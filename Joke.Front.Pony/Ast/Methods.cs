using System.Collections.Generic;

using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Methods : Node
    {
        public Methods(PonyTokenSpan span, IReadOnlyList<Method> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<Method> Items { get; }
    }
}
