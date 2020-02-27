using Joke.Front.Pony.ParseTree;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing.Parts
{
    public class IsPart : InfixPart
    {
        public IsPart(PonyTokenSpan span, PtBinaryKind kind, PtExpression expression)
            : base(span, kind)
        {
            Expression = expression;
        }

        public PtExpression Expression { get; }
    }
}
