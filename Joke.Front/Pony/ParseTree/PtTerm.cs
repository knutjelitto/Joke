using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtTerm : PtExpression
    {
        public PtTerm(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
