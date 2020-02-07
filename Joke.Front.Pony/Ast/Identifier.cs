using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Identifier : Expression
    {
        public Identifier(ISpan span)
            : base(span)
        {
        }
    }
}
