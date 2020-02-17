using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class LambdaParameters : Node
    {
        public LambdaParameters(TSpan span, IReadOnlyList<LambdaParameter> parameters)
            : base(span)
        {
            Items = parameters;
        }

        public IReadOnlyList<LambdaParameter> Items { get; }
    }
}
