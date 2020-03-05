using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class TypeParameterList : Collection<TypeParameter>
    {
        public TypeParameterList(TokenSpan span, IReadOnlyList<TypeParameter> items)
            : base(span, items)
        {
        }
    }
}
