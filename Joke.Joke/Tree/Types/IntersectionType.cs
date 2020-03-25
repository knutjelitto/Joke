using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class IntersectionType : IType
    {
        public IntersectionType(TokenSpan span, IReadOnlyList<IType> items)
        {
            Span = span;
            Items = items;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<IType> Items { get; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
