using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class GroupedType : Type
    {
        public GroupedType(TSpan span, IReadOnlyList<Type> types)
            : base(span)
        {
            Types = types;
        }

        public IReadOnlyList<Type> Types { get; }
    }
}
