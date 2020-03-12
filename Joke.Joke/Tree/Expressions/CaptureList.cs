using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class CaptureList : Collection<ICapture>, IAny
    {
        public CaptureList(TokenSpan span, IReadOnlyList<ICapture> items)
            : base(items)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
