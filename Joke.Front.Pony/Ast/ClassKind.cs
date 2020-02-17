using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public enum ClassKind
    {
        Missing,

        Type,
        Interface,
        Trait,
        Primitive,
        Struct,
        Class,
        Actor,
    }
}
