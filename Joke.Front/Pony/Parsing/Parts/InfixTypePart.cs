using Joke.Front.Pony.ParseTree;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing.Parts
{
    public class InfixTypePart : PtNode
    {
        public InfixTypePart(PonyTokenSpan span, PtInfixTypeKind kind, PtType right)
            : base(span)
        {
            Kind = kind;
            Right = right;
        }

        public PtInfixTypeKind Kind { get; }
        public PtType Right { get; }
    }
}
