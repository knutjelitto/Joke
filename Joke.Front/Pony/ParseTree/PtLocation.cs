using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLocation : PtExpression
    {
        public PtLocation(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
