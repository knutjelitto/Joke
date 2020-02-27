using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLambdaTypeParameters : PtNode
    {
        public PtLambdaTypeParameters(PonyTokenSpan span, IReadOnlyList<PtType> types)
            : base(span)
        {
            Types = types;
        }

        public IReadOnlyList<PtType> Types { get; }
    }
}
