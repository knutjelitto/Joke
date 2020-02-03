using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Joke.Ast
{
    public abstract class Member : Item
    {
        public Member(SourceSpan span)
            : base(span)
        {
        }
    }
}
