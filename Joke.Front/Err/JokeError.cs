namespace Joke.Front.Err
{
    public class JokeError : Error
    {
        public JokeError(IDescription err)
            : base(Severity.Error, err)
        {
        }
    }
}
