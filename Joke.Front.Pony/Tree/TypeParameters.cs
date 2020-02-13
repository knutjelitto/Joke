using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
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
