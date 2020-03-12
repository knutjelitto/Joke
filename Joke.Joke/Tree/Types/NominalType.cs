using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class NominalType : IType
    {
        public NominalType(TokenSpan span, QualifiedIdentifier name, TypeList? arguments, Cap? cap, Ephm? ephm, Aliased? aliased)
        {
            Span = span;
            Name = name;
            Arguments = arguments;
            Cap = cap;
            Ephm = ephm;
            Aliased = aliased;
        }

        public TokenSpan Span { get; }
        public QualifiedIdentifier Name { get; }
        public TypeList? Arguments { get; }
        public Cap? Cap { get; }
        public Ephm? Ephm { get; }
        public Aliased? Aliased { get; }
    }
}
