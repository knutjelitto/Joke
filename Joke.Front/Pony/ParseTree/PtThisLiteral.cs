using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtThisLiteral : PtExpression
    {
        public PtThisLiteral(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
