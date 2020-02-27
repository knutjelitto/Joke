using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtCases : PtNode
    {
        public PtCases(PonyTokenSpan span, IReadOnlyList<PtCase> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<PtCase> Items { get; }
    }
}
