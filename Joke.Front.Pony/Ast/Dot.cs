using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Dot : PostfixPart
    {
        public Dot(TokenSpan span, Identifier member)
            : base(span)
        {
            Member = member;
        }

        public Identifier Member { get; }
    }
}
