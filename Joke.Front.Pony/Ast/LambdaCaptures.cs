using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class LambdaCaptures : Node
    {
        public LambdaCaptures(PonyTokenSpan span, IReadOnlyList<LambdaCapture> captures)
            : base(span)
        {
            Captures = captures;
        }

        public IReadOnlyList<LambdaCapture> Captures { get; }
    }
}
