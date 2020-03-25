using Joke.Joke.Decoding;

namespace Joke.Joke.Err
{
    public class ErrorsHelper
    {
        public ErrorsHelper(Errors errors)
        {
            Errors = errors;
        }

        public Errors Errors { get; }

        public void Add(ISourceSpan span, ErrNo errNo, string msg)
        {
            Errors.Add(new JokeError(errNo, new AtOffset(span, msg)));
        }

        public void Add(ITokenSpan span, ErrNo errNo, string msg)
        {
            Errors.Add(new JokeError(errNo, new AtTokens(span, msg)));
        }

        public void Add(IToken token, ErrNo errNo, string msg)
        {
            Errors.Add(new JokeError(errNo, new AtToken(token, msg)));
        }
    }
}
