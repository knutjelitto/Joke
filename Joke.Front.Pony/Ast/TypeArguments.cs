using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class TypeArguments : Node
    {
        public TypeArguments(TokenSpan span, IReadOnlyList<Type> arguments)
            : base(span)
        {
            Arguments = arguments;
        }

        public IReadOnlyList<Type> Arguments { get; }
    }
}
