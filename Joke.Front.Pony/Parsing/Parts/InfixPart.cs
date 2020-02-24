using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Syntax.Parts
{
    public class InfixPart : Node
    {
        public InfixPart(PonyTokenSpan span, BinaryKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public BinaryKind Kind { get; }
    }
}
