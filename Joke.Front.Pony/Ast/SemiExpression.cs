using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class SemiExpression : Expression
    {
        public SemiExpression(TokenSpan span, Expression expression)
            : base(span)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
