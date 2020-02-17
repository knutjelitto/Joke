using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Tree
{
    public class Dot : PostfixPart
    {
        public Dot(TSpan span, Identifier member)
            : base(span)
        {
            Member = member;
        }

        public Identifier Member { get; }
    }
}
