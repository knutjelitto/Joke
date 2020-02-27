using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLambdaCaptures : PtNode
    {
        public PtLambdaCaptures(PonyTokenSpan span, IReadOnlyList<PtLambdaCapture> captures)
            : base(span)
        {
            Captures = captures;
        }

        public IReadOnlyList<PtLambdaCapture> Captures { get; }
    }
}
