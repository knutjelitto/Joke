using System.Collections.Generic;

using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtMethods : PtNode
    {
        public PtMethods(PonyTokenSpan span, IReadOnlyList<PtMethod> items)
            : base(span)
        {
            Items = items;
        }

        public IReadOnlyList<PtMethod> Items { get; }
    }
}
