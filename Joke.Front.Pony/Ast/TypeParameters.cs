using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class TypeParameters : Node
    {
        public TypeParameters(TokenSpan span, IReadOnlyList<TypeParameter> parameters)
            : base(span)
        {
            Parameters = parameters;
        }

        public IReadOnlyList<TypeParameter> Parameters { get; }
    }
}
