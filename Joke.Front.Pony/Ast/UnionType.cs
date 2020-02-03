using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class UnionType : Type
    {
        public UnionType(ISpan span, IReadOnlyList<Type> types)
            : base(span)
        {
            Types = types.ToArray();
        }

        public IReadOnlyList<Type> Types { get; }
    }
}
