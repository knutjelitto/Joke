using Joke.Joke.Decoding;
using Joke.Joke.Tools;

namespace Joke.Joke.Tree
{
    public class Unit : INamedMember
    {
        public Unit(TokenSpan span, String? packageDoc, MemberList items)
        {
            Span = span;
            PackageDoc = packageDoc;
            Items = items;
            Members = new DistinctList<INamed, Class>();
        }

        public TokenSpan Span { get; }
        public String? PackageDoc { get; }
        public MemberList Items { get; }

        // -- 
        public DistinctList<INamed, Class> Members { get; }

        public Identifier Name => throw new System.NotImplementedException();

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
