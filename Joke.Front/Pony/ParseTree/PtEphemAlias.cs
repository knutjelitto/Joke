using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtEphemAlias : PtNode
    {
        public PtEphemAlias(PonyTokenSpan span, PtEAKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public PtEAKind Kind { get; }
    }
}
