using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class MemberList : Collection<INamedMember>, IAny
    {
        public MemberList(TokenSpan span, IReadOnlyList<INamedMember> items)
            : base(items)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
