using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Identifier : IAny
    {
        public Identifier(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }

        public override string ToString()
        {
            return Span[0].Payload;
        }

        public override bool Equals(object? obj)
        {
            return ToString().Equals(obj?.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
