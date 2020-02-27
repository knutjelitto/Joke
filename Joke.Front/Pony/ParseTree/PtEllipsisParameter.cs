using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtEllipsisParameter : PtParameter
    {
        public PtEllipsisParameter(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
