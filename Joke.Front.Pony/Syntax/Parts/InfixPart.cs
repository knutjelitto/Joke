using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax.Parts
{
    public class InfixPart : Node
    {
        public InfixPart(TSpan span, BinaryKind kind)
            : base(span)
        {
            Kind = kind;
        }

        public BinaryKind Kind { get; }
    }
}
