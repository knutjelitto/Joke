using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ThisType : IType
    {
        public ThisType(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
