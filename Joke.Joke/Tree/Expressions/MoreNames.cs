using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class MoreNames : INamePattern
    {
        public MoreNames(TokenSpan span, IReadOnlyList<INamePattern> patterns)
        {
            Span = span;
            Patterns = patterns;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<INamePattern> Patterns { get; }
    }
}
