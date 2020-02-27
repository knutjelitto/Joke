using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLambdaParameters : PtNode
    {
        public PtLambdaParameters(PonyTokenSpan span, IReadOnlyList<PtLambdaParameter> parameters)
            : base(span)
        {
            Items = parameters;
        }

        public IReadOnlyList<PtLambdaParameter> Items { get; }
    }
}
