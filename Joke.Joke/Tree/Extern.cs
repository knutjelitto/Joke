using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Extern : INamedMember
    {
        public Extern(TokenSpan span, Identifier name, IReadOnlyList<IMember> members)
        {
            Span = span;
            Name = name;
            Members = members;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IReadOnlyList<IMember> Members { get; }
    }
}
