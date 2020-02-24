using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class TypeArguments : Node
    {
        public TypeArguments(PonyTokenSpan span, IReadOnlyList<Type> arguments)
            : base(span)
        {
            Arguments = arguments;
        }

        public IReadOnlyList<Type> Arguments { get; }
    }
}
