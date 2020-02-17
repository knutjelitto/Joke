using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class TypeArguments : Node
    {
        public TypeArguments(TSpan span, IReadOnlyList<TypeArgument> arguments)
            : base(span)
        {
            Arguments = arguments;
        }

        public IReadOnlyList<TypeArgument> Arguments { get; }
    }
}
