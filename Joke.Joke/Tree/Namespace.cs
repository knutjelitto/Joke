using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Namespace : IMember
    {
        public Namespace(TokenSpan span, QualifiedIdentifier name, MemberList members)
        {
            Span = span;
            Name = name;
            Members = members;
        }

        public TokenSpan Span { get; }
        public QualifiedIdentifier Name { get; }
        public MemberList Members { get; }
    }
}
