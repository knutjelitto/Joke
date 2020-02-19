using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Err
{
    public class AtToken : AtOffset
    {
        public AtToken(Source source, Token token, string msg) : base(source, token.Payload, msg)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}
