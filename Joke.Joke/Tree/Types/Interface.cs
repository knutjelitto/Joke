using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Interface : IType, IMember
    {
        public Interface(TokenSpan span, Identifier name, IType? provides, MemberList members)
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
