namespace Joke.Front.Pony.Err
{
    public class ParseError : Message
    {
        public ParseError(ErrorKind kind, string msg)
            : base(kind, msg)
        {
        }
    }
}
