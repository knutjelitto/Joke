using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtTypeArguments : PtNode
    {
        public PtTypeArguments(PonyTokenSpan span, IReadOnlyList<PtType> arguments)
            : base(span)
        {
            Arguments = arguments;
        }

        public IReadOnlyList<PtType> Arguments { get; }
    }
}
