using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class MemberList : Collection<IMember>, IAny
    {
        public MemberList(TokenSpan span, IReadOnlyList<IMember> items)
            : base(items)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
