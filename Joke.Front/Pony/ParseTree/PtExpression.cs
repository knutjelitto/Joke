using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public abstract class PtExpression : PtNode
    {
        protected PtExpression(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
