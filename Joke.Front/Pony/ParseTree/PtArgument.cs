using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public abstract class PtArgument : PtExpression
    {
        public PtArgument(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
