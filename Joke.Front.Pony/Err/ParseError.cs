namespace Joke.Front.Pony.Err
{
    public class ParseError : Error
    {
        public ParseError(ErrorKind kind, string msg)
            : base(kind, msg)
        {
        }
    }
}
