using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public enum MemberKind
    {
        Let,
        Var,
        Embed,

        New,
        Fun,
        Be,
    }
}
