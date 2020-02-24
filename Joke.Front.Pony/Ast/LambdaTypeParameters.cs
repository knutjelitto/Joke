using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class LambdaTypeParameters : Node
    {
        public LambdaTypeParameters(PonyTokenSpan span, IReadOnlyList<Type> types)
            : base(span)
        {
            Types = types;
        }

        public IReadOnlyList<Type> Types { get; }
    }
}
