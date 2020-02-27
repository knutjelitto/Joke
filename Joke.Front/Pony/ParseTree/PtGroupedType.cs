using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtGroupedType : PtType
    {
        public PtGroupedType(PonyTokenSpan span, IReadOnlyList<PtType> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<PtType> Items { get; }
    }
}
