using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class UnionType : IType
    {
        public UnionType(TokenSpan span, IReadOnlyList<IType> items)
        {
            Span = span;
            Items = items;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<IType> Items { get; }
    }
}
