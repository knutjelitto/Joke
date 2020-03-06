using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Extern : IMember
    {
        public Extern(TokenSpan span, IReadOnlyList<IMember> members)
        {
            Span = span;
            Members = members;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<IMember> Members { get; }
    }
}
