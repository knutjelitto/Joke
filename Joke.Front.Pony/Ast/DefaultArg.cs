using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class DefaultArg : Expression
    {
        public DefaultArg(TSpan span, Expression expression)
            : base(span)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
