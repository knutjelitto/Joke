using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax.Parts
{
    public class InfixTypePart : Node
    {
        public InfixTypePart(TSpan span, InfixTypeKind kind, Type right)
            : base(span)
        {
            Kind = kind;
            Right = right;
        }

        public InfixTypeKind Kind { get; }
        public Type Right { get; }
    }
}
