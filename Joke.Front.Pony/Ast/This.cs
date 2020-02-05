using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class This : Expression
    {
        public This(ISpan span)
            : base(span)
        {
        }
    }
}
