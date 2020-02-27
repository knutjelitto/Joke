using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public abstract class PtType : PtNode
    {
        protected PtType(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
