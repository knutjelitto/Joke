using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class String : Literal
    {
        public String(ISpan span)
            : base(span)
        {
        }
    }
}
