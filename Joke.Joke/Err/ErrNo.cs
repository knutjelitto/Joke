namespace Joke.Joke.Err
{
    public enum ErrNo
    {
        NoError,            // really no error
        LEX001,             // unknown character in source stream
        NoScanToken,        // no scan in toknizer
    }
}
