namespace Joke.Front.Pony.Ast
{
    public enum BinaryKind
    {
        Missing,

        Is,
        Isnt,
        As,

        And,
        Or,
        Xor,

        Plus,
        PlusUnsafe,
        PlusPartial,
        Minus,
        MinusUnsafe,
        MinusPartial,
        Multiply,
        MultiplyUnsafe,
        MultiplyPartial,
        Divide,
        DivideUnsafe,
        DividePartial,
        Mod,
        ModUnsafe,
        ModPartial,
        Rem,
        RemUnsafe,
        RemPartial,

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
