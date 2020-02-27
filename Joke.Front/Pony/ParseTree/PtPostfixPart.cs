using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtPostfixPart : PtExpression
    {
        public PtPostfixPart(PonyTokenSpan span) : base(span)
        {
        }
    }
}
