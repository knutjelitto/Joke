using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class LambdaParameters : Node
    {
        public LambdaParameters(PonyTokenSpan span, IReadOnlyList<LambdaParameter> parameters)
            : base(span)
        {
            Items = parameters;
        }

        public IReadOnlyList<LambdaParameter> Items { get; }
    }
}
