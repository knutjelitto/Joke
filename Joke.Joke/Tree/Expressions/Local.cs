using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Local : IExpression
    {
        public Local(TokenSpan span, LocalKind kind, Identifier name, IType? type)
        {
            Span = span;
            Kind = kind;
            Name = name;
            Type = type;
        }

        public TokenSpan Span { get; }
        public LocalKind Kind { get; }
        public Identifier Name { get; }
        public IType? Type { get; }
    }
}
