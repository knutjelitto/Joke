using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Chain : PostfixPart
    {
        public Chain(TSpan span, Identifier method)
            : base(span)
        {
            Method = method;
        }

        public Identifier Method { get; }
    }
}
