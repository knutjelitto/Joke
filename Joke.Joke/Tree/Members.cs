using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Members : Collection<IMember>
    {
        public Members(TokenSpan span, IReadOnlyList<IMember> items)
            : base(span, items)
        {
        }
    }
}
