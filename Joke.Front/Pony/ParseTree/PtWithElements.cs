using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtWithElements : PtNode
    {
        public PtWithElements(PonyTokenSpan span, IReadOnlyList<PtWithElement> elements)
            : base(span)
        {
            Elements = elements;
        }

        public IReadOnlyList<PtWithElement> Elements { get; }
    }
}
