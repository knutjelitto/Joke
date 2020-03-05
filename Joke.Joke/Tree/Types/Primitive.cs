using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Primitive : IType, IMember
    {
        public Primitive(TokenSpan span, Identifier name, IType? provides, MemberList members)
        {
            Span = span;
            Name = name;
            Provides = provides;
            Members = members;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IType? Provides { get; }
        public MemberList Members { get; }
    }
}
