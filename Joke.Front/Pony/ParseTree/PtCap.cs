using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtCap : PtType
    {
        public PtCap(PonyTokenSpan span, PtCapKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public PtCapKind Kind { get; }
    }
}
