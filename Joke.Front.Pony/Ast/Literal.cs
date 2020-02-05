using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Literal : Expression
    {
        public Literal(ISpan span)
            : base(span)
        {
        }
    }
}
