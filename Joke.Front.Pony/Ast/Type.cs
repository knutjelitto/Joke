using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class Type : Base
    {
        public Type(ISpan span)
            : base(span)
        {
        }
    }
}
