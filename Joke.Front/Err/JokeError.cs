namespace Joke.Front.Err
{
    public class JokeError : Error
    {
        public JokeError(ErrNo no, IDescription err)
            : base(Severity.Error, no, err)
        {
        }
    }
}
