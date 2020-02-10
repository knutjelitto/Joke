using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Parameters : Base
    {
        public Parameters(ISpan span, IReadOnlyList<Parameter> items, bool ellipsis)
            : base(span)
        {
            Items = items;
            Ellipsis = ellipsis;
        }

        public IReadOnlyList<Parameter> Items { get; }
        public Boolean Ellipsis { get; }
    }
}
