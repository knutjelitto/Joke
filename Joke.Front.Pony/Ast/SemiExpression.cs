using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class SemiExpression : Expression
    {
        public SemiExpression(TSpan span, Expression expression)
            : base(span)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
