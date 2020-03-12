using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class CaptureList : Collection<ICapture>
    {
        public CaptureList(TokenSpan span, IReadOnlyList<ICapture> items)
            : base(span, items)
        {
        }
    }
}
