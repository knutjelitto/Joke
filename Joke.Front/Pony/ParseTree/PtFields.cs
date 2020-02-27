using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtFields : PtNode
    {
        public PtFields(PonyTokenSpan span, IReadOnlyList<PtField> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<PtField> Items { get; }
    }
}
