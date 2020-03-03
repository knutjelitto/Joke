using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class QualifiedIdentifier
    {
        public QualifiedIdentifier(TokenSpan span, IReadOnlyList<Identifier> names)
        {
            Span = span;
            Names = names;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<Identifier> Names { get; }
    }
}
