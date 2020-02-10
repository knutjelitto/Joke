using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class ExternIdentifierString : ExternIdentifier
    {
        public ExternIdentifierString(ISpan span)
            : base(span)
        {
        }
    }
}
