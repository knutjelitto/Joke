using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public abstract class PtIds : PtNode
    {
        protected PtIds(PonyTokenSpan span) : base(span)
        {
        }
    }
}
