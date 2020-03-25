using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Local : IExpression, INamed
    {
        public Local(TokenSpan span, LocalKind kind, Identifier name, IType? type, String? doc)
        {
            Span = span;
            Kind = kind;
            Name = name;
            Type = type;
            Doc = doc;
        }

        public TokenSpan Span { get; }
        public LocalKind Kind { get; }
        public Identifier Name { get; }
        public IType? Type { get; }
        public String? Doc { get; }
    }
}
