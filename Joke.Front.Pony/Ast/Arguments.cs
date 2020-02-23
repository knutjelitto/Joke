using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class Arguments : Node
    {
        public Arguments(TokenSpan span, IReadOnlyList<Argument> arguments)
            : base(span)
        {
            Items = arguments;
        }

        public IReadOnlyList<Argument> Items { get; }
    }
}
