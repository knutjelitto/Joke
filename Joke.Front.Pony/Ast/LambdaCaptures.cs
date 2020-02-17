using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class LambdaCaptures : Node
    {
        public LambdaCaptures(TSpan span, IReadOnlyList<LambdaCapture> captures)
            : base(span)
        {
            Captures = captures;
        }

        public IReadOnlyList<LambdaCapture> Captures { get; }
    }
}
