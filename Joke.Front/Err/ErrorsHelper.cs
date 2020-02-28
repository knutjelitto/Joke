namespace Joke.Front.Err
{
    public class ErrorsHelper
    {
        public ErrorsHelper(Errors errors)
        {
            Errors = errors;
        }

        public Errors Errors { get; }

        public void Add(ITokenSpan span, string msg)
        {
            Errors.Add(new JokeError(new AtTokens(span, msg)));
        }
    }
}
