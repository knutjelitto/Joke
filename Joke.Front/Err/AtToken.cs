namespace Joke.Front.Err
{
    public class AtToken : AtOffset
    {
        public AtToken(ISource source, IToken token, string msg)
            : base(token.PayloadSpan, msg)
        {
            Token = token;
        }

        public IToken Token { get; }
    }
}
