using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class MethodMember : Member
    {
        public MethodMember(ISpan span, MemberKind kind)
            : base(span, kind)
        {
        }
    }
}
