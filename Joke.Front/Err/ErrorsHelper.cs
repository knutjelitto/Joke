namespace Joke.Front.Err
{
    public class ErrorsHelper
    {
        public ErrorsHelper(Errors errors)
        {
            Errors = errors;
        }

        public Errors Errors { get; }

        public void Add(ITokenSpan span, ErrNo no, string msg)
        {
            Errors.Add(new JokeError(no, new AtTokens(span, msg)));
        }

        public void Add(IToken token, ErrNo no, string msg)
        {
            Errors.Add(new JokeError(no, new AtToken(token, msg)));
        }
    }
}
