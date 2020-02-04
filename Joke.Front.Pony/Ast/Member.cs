using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class Member : Base
    {
        public Member(ISpan span)
            : base(span)
        {
        }
    }
}
