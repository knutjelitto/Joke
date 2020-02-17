using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Tree
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
