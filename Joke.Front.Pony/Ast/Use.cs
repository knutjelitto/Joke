using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class Use : Item
    {
        public Use(ISpan span, Identifier? name)
            : base(span)
        {
            Name = name;
        }

        public Identifier? Name { get; }
    }
}
