using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Unit : IMember
    {
        public Unit(TokenSpan span, MemberList members)
        {
            Span = span;
            Members = members;
        }

        public TokenSpan Span { get; }
        public MemberList Members { get; }
    }
}
