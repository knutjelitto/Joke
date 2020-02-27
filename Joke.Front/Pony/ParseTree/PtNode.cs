using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public abstract class PtNode
    {
        protected PtNode(PonyTokenSpan span)
        {
            Span = span;
        }

        public PonyTokenSpan Span { get; }
    }
}
