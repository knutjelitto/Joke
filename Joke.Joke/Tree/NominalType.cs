using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class NominalType : IType
    {
        public NominalType(TokenSpan span, QualifiedIdentifier name, Types? arguments)
        {
            Span = span;
            Name = name;
            Arguments = arguments;
        }

        public TokenSpan Span { get; }
        public QualifiedIdentifier Name { get; }
        public Types? Arguments { get; }
    }
}
