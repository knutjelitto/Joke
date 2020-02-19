namespace Joke.Front.Pony.Err
{
    public class ParseError : Error
    {
        public ParseError(IDescription err)
            : base(Severity.Error, err)
        {
        }
    }
}
