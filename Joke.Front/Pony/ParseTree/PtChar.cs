using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtChar : PtExpression
    {
        public PtChar(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
