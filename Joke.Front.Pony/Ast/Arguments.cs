using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Arguments : Node
    {
        public Arguments(TSpan span, IReadOnlyList<Argument> arguments)
            : base(span)
        {
            Items = arguments;
        }

        public IReadOnlyList<Argument> Items { get; }
    }
}
