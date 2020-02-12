namespace Joke.Front.Pony.Lex
{
    public enum TK
    {
        Eof,

        Identifier,

        String,
        DocString,
        Char,
        Int,
        Float,

        LBrace,             // '{'
        RBrace,             // '}'
        LParen,             // '('
        LParenNew,          // <nl>'('
        RParen,             // ')'
        LSquare,            // '['
        LSquareNew,         // <nl>'['
        RSquare,            // ']'

        Comma,              // ','
        Colon,              // ':'
        Semi,               // ';'
        Question,           // '?'
        Pipe,               // '|'
        Ephemeral,          // '^'
        ISectType,          // '&'
        Constant,           // '#'

        At,                 // '@'
        AtLBrace,           // '@{'

        Dot,                // '.'
        Chain,              // '.>'
        Ellipsis,           // '...'

        Assign,             // '='
        DblArrow,           // '=>'
        Eq,                 // '=='
        EqTilde,            // '==~'

        Aliased,            // '!'
        Ne,                 // '!='
        NeTilde,            // '!=~'

        Lt,                 // '<'
        Subtype,            // '<:'
        LtTilde,            // '<~'
        Le,                 // '<='
        LeTilde,            // '<=~'
        LShift,             // '<<'
        LShiftTilde,        // '<<~'

        Gt,                 // '>'
        GtTilde,            // '>~'
        Ge,                 // '>='
        GeTilde,            // '>=~'
        RShift,             // '>>'
        RShiftTilde,        // '>>~'

        Plus,               // '+'
        PlusTilde,          // '+~'

        Minus,              // '-'
        MinusNew,           // <nl>'-'
        Arrow,              // '->'
        MinusTilde,         // '-~'
        MinusTildeNew,      // <nl>'-~'

        Divide,             // '/'
        DivideTilde,        // '/~'

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

        CapRead,            // '#read'
        CapSend,            // '#send'
        CapShare,           // '#share'
        CapAlias,           // '#alias'
        CapAny,             // '#any'
    }
}
