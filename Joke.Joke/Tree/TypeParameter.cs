using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class TypeParameter
    {
        public TypeParameter(TokenSpan span, Identifier name, IType? type, IType? @default)
        {
            Span = span;
            Name = name;
            Type = type;
            Default = @default;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IType? Type { get; }
        public IType? Default { get; }
    }
}
