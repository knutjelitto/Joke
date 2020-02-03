using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class GroupedType : Type
    {
        public GroupedType(ISpan span, IReadOnlyList<Type> types)
            : base(span)
        {
            Types = types;
        }

        public IReadOnlyList<Type> Types { get; }
    }
}
