using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class TypeParameters : Node
    {
        public TypeParameters(TSpan span, IReadOnlyList<TypeParameter> parameters)
            : base(span)
        {
            Parameters = parameters;
        }

        public IReadOnlyList<TypeParameter> Parameters { get; }
    }
}
