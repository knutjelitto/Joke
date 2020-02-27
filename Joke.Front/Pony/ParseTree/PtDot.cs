using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtDot : PtPostfixPart
    {
        public PtDot(PonyTokenSpan span, PtIdentifier member)
            : base(span)
        {
            Member = member;
        }

        public PtIdentifier Member { get; }
    }
}
