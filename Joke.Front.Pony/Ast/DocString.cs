using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class DocString : Item
    {
        public DocString(ISpan span)
            : base(span)
        {
        }
    }
}
