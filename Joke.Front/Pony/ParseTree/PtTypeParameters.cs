using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtTypeParameters : PtNode
    {
        public PtTypeParameters(PonyTokenSpan span, IReadOnlyList<PtTypeParameter> parameters)
            : base(span)
        {
            Parameters = parameters;
        }

        public IReadOnlyList<PtTypeParameter> Parameters { get; }
    }
}
