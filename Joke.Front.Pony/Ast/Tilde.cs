using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Tilde : PostfixPart
    {
        public Tilde(PonyTokenSpan span, Identifier method)
            : base(span)
        {
            Method = method;
        }

        public Identifier Method { get; }
    }
}
