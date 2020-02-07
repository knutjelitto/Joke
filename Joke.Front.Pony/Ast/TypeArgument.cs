using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class TypeArgument : Base
    {
        public TypeArgument(ISpan span)
            : base(span)
        {
        }
    }
}
