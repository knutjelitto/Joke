using Joke.Joke.Decoding;

namespace Joke.Joke.Err
{
    public class AtToken : AtOffset
    {
        public AtToken(IToken token, string msg)
            : base(token.PayloadSpan, msg)
        {
            Token = token;
        }

        public IToken Token { get; }
    }
}
