using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public abstract class PtLambdaCapture : PtNode
    {
        protected PtLambdaCapture(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
