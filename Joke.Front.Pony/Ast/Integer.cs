using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Integer : Literal
    {
        public Integer(ISpan span)
            : base(span)
        {
        }
    }
}
