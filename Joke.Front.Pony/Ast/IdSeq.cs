using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class IdSeq : Base
    {
        public IdSeq(ISpan span)
            : base(span)
        {
        }
    }
}
