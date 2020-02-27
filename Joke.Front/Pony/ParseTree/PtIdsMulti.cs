using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtIdsMulti : PtIds
    {
        public PtIdsMulti(PonyTokenSpan span, IReadOnlyList<PtIds> names)
            : base(span)
        {
            Names = names;
        }

        public IReadOnlyList<PtIds> Names { get; }
    }
}
