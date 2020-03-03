using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Namespace
    {
        public Namespace(TokenSpan span, QualifiedIdentifier name, Members members)
        {
            Span = span;
            Name = name;
            Members = members;
        }

        public TokenSpan Span { get; }
        public QualifiedIdentifier Name { get; }
        public Members Members { get; }
    }
}
