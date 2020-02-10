using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class ExternIdentifierPlain : ExternIdentifier
    {
        public ExternIdentifierPlain(ISpan span)
            : base(span)
        {
        }
    }
}
