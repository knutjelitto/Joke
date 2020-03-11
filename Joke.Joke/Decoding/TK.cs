namespace Joke.Joke.Decoding
{
    public enum TK
    {
        Missing,
        Eof,
        Wildcard,
        Identifier,
        String,
        DocString,
        Char,
        Integer,
        Float,

        LBrace,             // '{'
        RBrace,             // '}'
        LParen,             // '('
        RParen,             // ')'
        LSquare,            // '['
        RSquare,            // ']'
        Comma,              // ','
        Colon,              // ':'
        Semi,               // ';'
        Tilde,              // '~'
        Question,           // '?'
        Pipe,               // '|'
        Hat,                // '^'
        Amper,              // '&'
        Hash,               // '#'

        At,                 // '@'
        AtLBrace,           // '@{'

        Dot,                // '.'
        Chain,              // '.>'
        Ellipsis,           // '...'

        Assign,             // '='
        DblArrow,           // '=>'
        Eq,                 // '=='

        Exclamation,        // '!'
        Ne,                 // '!='

        Lt,                 // '<'
        Subtype,            // '<:'
        Le,                 // '<='
        LShift,             // '<<'

        Gt,                 // '>'
        Ge,                 // '>='
        RShift,             // '>>'

        Plus,               // '+'

        Minus,              // '-'
        Arrow,              // '->'

        Divide,             // '/'

        Multiply,           // '*'

        Rem,                // '%'
        Mod,                // '%%'

        Actor,              // 'actor'
        Addressof,          // 'addressof'
        And,                // 'and'
        As,                 // 'as'
        Be,                 // 'be'
        Box,                // 'box'
        Break,              // 'break'
        Class,              // 'class'
        CompileError,       // 'compile_error'
        CompileIntrinsic,   // 'compile_intrinsic'
        Continue,           // 'continue'
        Digestof,           // 'digestof'
        Do,                 // 'do'
        Else,               // 'else'
        Elseif,             // 'elseif'
        Embed,              // 'embed'
        End,                // 'end'
        Error,              // 'error'
        Extern,             // 'extern'
        False,              // 'false'
        For,                // 'for'
        Fun,                // 'fun'
        If,                 // 'if'
        In,                 // 'in'
        Interface,          // 'interface'
        Is,                 // 'is'
        Isnt,               // 'isnt'
        Iso,                // 'iso'
        Lambda,             // 'lambda'
        Let,                // 'let'
        Loc,                // '__loc'
        Match,              // 'match'
        Namespace,          // 'namespace'
        New,                // 'new'
        Not,                // 'not'
        Or,                 // 'or'
        Object,             // 'object'
        Primitive,          // 'primitive'
        Ref,                // 'ref'
        Repeat,             // 'repeat'
        Return,             // 'return'
        Struct,             // 'struct'
        Tag,                // 'tag'
        Then,               // 'then'
        This,               // 'this'
        Trait,              // 'trait'
        Trn,                // 'trn'
        True,               // 'true'
        Try,                // 'try'
        Type,               // 'type'
        Until,              // 'until'
        Use,                // 'use'
        Val,                // 'val'
        Var,                // 'var'
        When,               // 'when'
        Where,              // 'where'
        While,              // 'while'
        With,               // 'with'
        Xor,                // 'xor'
    }
}
