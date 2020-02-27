using Joke.Front.Pony.ParseTree;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing.Parts
{
    public class BinaryPart : InfixPart
    {
        public BinaryPart(PonyTokenSpan span, PtBinaryKind kind, PtExpression right)
            : base(span, kind)
        {
            Right = right;
        }

        public PtExpression Right { get; }
    }
}
