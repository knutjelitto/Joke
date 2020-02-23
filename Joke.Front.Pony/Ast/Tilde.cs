using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Tilde : PostfixPart
    {
        public Tilde(TokenSpan span, Identifier method)
            : base(span)
        {
            Method = method;
        }

        public Identifier Method { get; }
    }
}
