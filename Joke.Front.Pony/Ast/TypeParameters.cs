using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class TypeParameters : Base
    {
        public TypeParameters(ISpan span, IReadOnlyList<TypeParameter> parameters)
            : base(span)
        {
            Parameters = parameters;
        }

        public IReadOnlyList<TypeParameter> Parameters { get; }
    }
}
