using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Aliased : IType
    {
        public Aliased(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
