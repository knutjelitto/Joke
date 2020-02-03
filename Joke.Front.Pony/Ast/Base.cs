using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class Base
    {
        public Base(ISpan span)
        {
            Span = span;
        }

        public ISpan Span { get; }
    }
}
