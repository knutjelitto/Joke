using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class TypeParameterList : Collection<TypeParameter>, IAny
    {
        public TypeParameterList(TokenSpan span, IReadOnlyList<TypeParameter> items)
            : base(items)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
