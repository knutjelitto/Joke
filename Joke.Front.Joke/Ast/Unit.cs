using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Joke.Ast
{
    public class Unit : Item
    {
        public Unit(SourceSpan span)
            : base(span)
        {
        }
    }
}
