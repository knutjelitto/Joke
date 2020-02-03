using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Joke.Ast
{
    public class Identifier : Item
    {
        public Identifier(SourceSpan span)
            : base(span)
        {
        }
    }
}
