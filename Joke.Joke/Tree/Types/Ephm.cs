using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Ephm : IType
    {
        public Ephm(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
