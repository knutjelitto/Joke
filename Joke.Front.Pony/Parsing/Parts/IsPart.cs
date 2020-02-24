using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Syntax.Parts
{
    public class IsPart : InfixPart
    {
        public IsPart(PonyTokenSpan span, BinaryKind kind, Expression expression)
            : base(span, kind)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
