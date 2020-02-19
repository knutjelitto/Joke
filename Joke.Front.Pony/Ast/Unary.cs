using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Unary : Expression
    {
        public Unary(TokenSpan span, UnaryKind kind, Expression expression)
            : base(span)
        {
            Kind = kind;
            Expression = expression;
        }

        public UnaryKind Kind { get; }
        public Expression Expression { get; }
    }
}
