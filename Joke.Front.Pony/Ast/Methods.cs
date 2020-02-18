using System.Collections.Generic;

using Joke.Front.Pony.Lex;

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
