namespace Joke.Front.Pony.Lex
{
    public enum TK
    {
        Eof,

        Identifier,

        String,
        DocString,

        LBrace,     // '{'
        RBrace,     // '}'
        LParen,     // '('
        LParenNew,  // <nl>'('
        RParen,     // ')'
        LSquare,    // '['
        LSquareNew, // <nl>'['
        RSquare,    // ']'

        Colon,      // ':'

        DblArrow,   // '=>'
        EqTilde,    // '==~'
        Eq,         // '=='
        Assign,     // '='

        Actor,
        As,
        Be,
        Box,
        Break,
        Class,
        CompileError,
        CompileIntrinsic,
        Continue,
        Consume,
        DigestOf,
        Do,
        Else,
        Elseif,
        Embed,
        End,
        Error,
        For,
        Fun,
        If,
        Ifdef,
        In,
        Interface,
        Is,
        Isnt,
        Iso,
        Lambda,
        Let,
        Match,
        New,
        Not,
        Object,
        Primitive,
        Recover,
        Ref,
        Repeat,
        Return,
        Struct,
        Tag,
        Then,
        This,
        Trait,
        Trn,
        Try,
        Type,
        Until,
        Use,
        Var,
        Val,
        Where,
        While,
        With,
    }
}
