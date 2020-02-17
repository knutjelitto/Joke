using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public enum UnaryOpKind
    {
        Missing,

        Addressof,
        Digestof,
        Not,
        Minus,
        MinusUnsafe,
    }
}
