using Joke.Front.Pony.Ast;
using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Syntax.Parts
{
    public class IsPart : InfixPart
    {
        public IsPart(TSpan span, BinaryKind kind, Expression expression)
            : base(span, kind)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
