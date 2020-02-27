using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtDotIdentifier : PtIdentifier
    {
        public PtDotIdentifier(PonyTokenSpan span, PtIdentifier before, PtIdentifier after)
            : base(span)
        {
            Before = before;
            After = after;
        }

        public PtIdentifier Before { get; }
        public PtIdentifier After { get; }
    }
}
