namespace Joke.Front.Pony.Lexing
{
    public enum TK
    {
        Missing,

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
        Tilde,              // '~'
        Colon,              // ':'
        Semi,               // ';'
        Question,           // '?'
        Pipe,               // '|'
        Ephemeral,          // '^'
        ISectType,          // '&'
        Constant,           // '#'
        Backslash,          // '\'

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

        Multiply,           // '*'
        MultiplyTilde,      // '*~'

        Rem,                // '%'
        RemTilde,           // '%~'
        Mod,                // '%%'
        ModTilde,           // '%%~'

        Not,                // 'not'
        And,                // 'and'
        Or,                 // 'or'
        Xor,                // 'xor'

        True,               // 'true'
        False,              // 'false'

        Addressof,
        DigestOf,
        Location,           // '__loc'

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
        Iftype,
        In,
        Interface,
        Is,
        Isnt,
        Iso,
        Lambda,
        Let,
        Match,
        New,
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
