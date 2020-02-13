using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public enum BinaryOpKind
    {
        Missing,

        And,
        Or,
        Xor,

        Plus,
        PlusUnsafe,
        Minus,
        MinusUnsafe,
        Multiply,
        MultiplyUnsafe,
        Divide,
        DivideUnsafe,
        Mod,
        ModUnsafe,
        Rem,
        RemUnsafe,

        LShift,
        LShiftUnsafe,
        RShift,
        RShiftUnsafe,

        Eq,
        EqUnsafe,
        Ne,
        NeUnsafe,
        Lt,
        LtUnsafe,
        Le,
        LeUnsafe,
        Gt,
        GtUnsafe,
        Ge,
        GeUnsafe,
    }
}
