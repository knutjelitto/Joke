using Joke.Joke.Decoding;

namespace Joke.Joke.Err
{
    public class AtTokens : AtOffset
    {
        public AtTokens(ITokenSpan tokens, string msg)
            : base(tokens.PayloadSpan, msg)
        {
            Tokens = tokens;
        }

        public ITokenSpan Tokens { get; }
    }
}
