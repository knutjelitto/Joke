using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Guard : Expression
    {
        public Guard(TSpan span, Expression expression)
            : base(span)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
