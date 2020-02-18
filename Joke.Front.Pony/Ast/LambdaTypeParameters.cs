using Joke.Front.Pony.Lex;
using System.Collections.Generic;

namespace Joke.Front.Pony.Ast
{
    public class LambdaTypeParameters : Node
    {
        public LambdaTypeParameters(TSpan span, IReadOnlyList<Type> types)
            : base(span)
        {
            Types = types;
        }

        public IReadOnlyList<Type> Types { get; }
    }
}
