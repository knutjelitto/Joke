namespace Joke.Front.Err
{
    public enum ErrNo
    {
        NoError,            // really no error
        NotYetToken,        // no yet in tokenizer
        NoScanToken,        // no scan in toknizer
        NotYetParse,        // no yet in parser
        NoScanParse,        // no scan in parser
        Err001,             // no partial binary operator
        Err002,             // nonassociating binary ops
        Err003,             // nonsense alias
        Err004,             // nonsense ellipsis in ffi declaration
        Err005,             // nonsense ffi return type
        Load001,
    }
}
