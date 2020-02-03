using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class IntersectionType : Type
    {
        public IntersectionType(ISpan span, IEnumerable<Type> types)
            : base(span)
        {
            Types = types.ToArray();
        }

        public IReadOnlyList<Type> Types { get; }
    }
}
