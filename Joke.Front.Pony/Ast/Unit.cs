using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Unit : Base
    {
        public Unit(ISpan span, DocString? doc, IEnumerable<Item> items)
            : base(span)
        {
            Doc = doc;
            Items = items.ToArray();
        }

        public IReadOnlyList<Item> Items { get; }
        public DocString? Doc { get; }
    }
}
