using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public enum IffKind
    {
        Missing,

        Iff,
        IffDef,
        IffType,
        ElseIff,
        ElseIffDef,
        ElseIffType,
    }
}
