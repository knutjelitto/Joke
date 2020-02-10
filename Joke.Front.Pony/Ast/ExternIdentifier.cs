using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class ExternIdentifier : Base
    {
        public ExternIdentifier(ISpan span) : base(span)
        {
        }
    }
}
