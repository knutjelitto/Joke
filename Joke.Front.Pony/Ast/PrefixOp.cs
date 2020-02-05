using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public enum PrefixOp
    {
        Not,
        Neg,
        NegUnsafe,

        DigestOf,
        AddressOf,
    }
}
