using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class TypeParameters : Collection<TypeParameter>
    {
        public TypeParameters(TokenSpan span, IReadOnlyList<TypeParameter> items)
            : base(span, items)
        {
        }
    }
}
