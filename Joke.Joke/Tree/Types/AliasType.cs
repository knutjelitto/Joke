using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class AliasType : INamedMember, IType
    {
        public AliasType(TokenSpan span, Identifier name, IType type)
        {
            Span = span;
            Name = name;
            Type = type;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IType Type { get; }
    }
}
