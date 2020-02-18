using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Err
{
    public class AtTokenError : Error
    {
        public AtTokenError(ErrorKind kind, Token token, string msg) : base(kind, msg)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}
