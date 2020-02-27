using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtAnnotations : PtNode
    {
        public PtAnnotations(PonyTokenSpan span, IReadOnlyList<PtIdentifier> names)
            : base(span)
        {
            Names = names;
        }

        public IReadOnlyList<PtIdentifier> Names { get; }
    }
}
