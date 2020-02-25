namespace Joke.Front.Pony.Lexing
{
    public class PonyTokenSpan : TokenSpan<PonyToken>
    {
        public PonyTokenSpan(PonyTokens tokens, int start, int next)
            : base(tokens, start, next)
        {
        }
    }
}
