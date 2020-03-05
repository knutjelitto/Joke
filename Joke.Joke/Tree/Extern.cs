using Joke.Joke.Decoding;
using System;
using System.Collections.Generic;
using System.Text;

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
