using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax.Parts
{
    public class BinaryPart : InfixPart
    {
        public BinaryPart(TSpan span, BinaryKind kind, Expression right)
            : base(span, kind)
        {
            Right = right;
        }

        public Expression Right { get; }
    }
}
