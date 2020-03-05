using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class MemberList : Collection<IMember>
    {
        public MemberList(TokenSpan span, IReadOnlyList<IMember> items)
            : base(span, items)
        {
        }
    }
}
