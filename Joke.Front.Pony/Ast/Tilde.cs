using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Tilde : PostfixPart
    {
        public Tilde(TSpan span, Identifier method)
            : base(span)
        {
            Method = method;
        }

        public Identifier Method { get; }
    }
}
