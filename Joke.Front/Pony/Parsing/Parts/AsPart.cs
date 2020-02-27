using Joke.Front.Pony.ParseTree;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing.Parts
{
    public class AsPart : InfixPart
    {
        public AsPart(PonyTokenSpan span, PtType type)
            : base(span, PtBinaryKind.As)
        {
            Type = type;
        }

        public PtType Type { get; }
    }
}
