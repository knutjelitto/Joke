using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class OneName : INamePattern
    {
        public OneName(TokenSpan span, Identifier name)
        {
            Span = span;
            Name = name;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
    }
}
