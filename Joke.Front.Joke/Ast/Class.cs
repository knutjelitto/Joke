using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Joke.Ast
{
    public class Class : Member
    {
        public Class(SourceSpan span)
            : base(span)
        {
        }
    }
}
