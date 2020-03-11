using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Cap : IType
    {
        public Cap(TokenSpan span, CapKind kind)
        {
            Span = span;
            Kind = kind;
        }

        public TokenSpan Span { get; }
        public CapKind Kind { get; }
    }
}
