using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public abstract class PtParameter : PtNode
    {
        public PtParameter(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
