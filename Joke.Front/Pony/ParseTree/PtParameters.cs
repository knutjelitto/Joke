using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtParameters : PtNode
    {
        public PtParameters(PonyTokenSpan span, IReadOnlyList<PtParameter> parameters)
            : base(span)
        {
            Items = parameters;
        }

        public IReadOnlyList<PtParameter> Items { get; }
    }
}
