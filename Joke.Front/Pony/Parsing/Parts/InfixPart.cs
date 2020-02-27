using Joke.Front.Pony.ParseTree;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing.Parts
{
    public class InfixPart : PtNode
    {
        public InfixPart(PonyTokenSpan span, PtBinaryKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public PtBinaryKind Kind { get; }
    }
}
