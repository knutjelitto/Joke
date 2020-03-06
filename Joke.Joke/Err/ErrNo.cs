namespace Joke.Joke.Err
{
    public enum ErrNo
    {
        NoError,            // really no error
        Lex001,             // unknown character in source stream
        Scan001,            // unkown token in token stream, expected ``{}´´ but got ``{}´´
        Scan002,            // binary operators have no precedence, use ( ) to group binary expressions
        Scan003,            // inconclusive parse, beeing not at eof
        Scan004,            // expected ``{}´´, found ``{}´´
        NoScanToken,        // no scan in toknizer
    }
}
