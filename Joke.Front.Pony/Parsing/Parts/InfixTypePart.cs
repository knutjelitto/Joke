using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Parsing.Parts
{
    public class InfixTypePart : Node
    {
        public InfixTypePart(PonyTokenSpan span, InfixTypeKind kind, Type right)
            : base(span)
        {
            Kind = kind;
            Right = right;
        }

        public InfixTypeKind Kind { get; }
        public Type Right { get; }
    }
}
