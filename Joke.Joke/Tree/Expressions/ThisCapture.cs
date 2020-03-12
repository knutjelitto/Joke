using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ThisCapture : ICapture
    {
        public ThisCapture(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
