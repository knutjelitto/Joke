using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtPartial : PtNode
    {
        public PtPartial(PonyTokenSpan span)
            : base(span)
        {
        }
    }
}
