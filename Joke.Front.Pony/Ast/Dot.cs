using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Dot : PostfixPart
    {
        public Dot(PonyTokenSpan span, Identifier member)
            : base(span)
        {
            Member = member;
        }

        public Identifier Member { get; }
    }
}
