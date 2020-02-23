using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Err
{
    public class AtToken : AtOffset
    {
        public AtToken(ISource source, Token token, string msg)
            : base(source, token.Payload, token.Next - token.Payload, msg)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}
