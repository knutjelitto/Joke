using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
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
