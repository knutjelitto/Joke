using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Unit : IMember
    {
        public Unit(TokenSpan span, String? packageDoc, MemberList members)
        {
            Span = span;
            PackageDoc = packageDoc;
            Members = members;
        }

        public TokenSpan Span { get; }
        public String? PackageDoc { get; }
        public MemberList Members { get; }
    }
}
