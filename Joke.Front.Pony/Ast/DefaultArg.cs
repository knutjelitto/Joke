using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class DefaultArg : Expression
    {
        public DefaultArg(TokenSpan span, Expression expression)
            : base(span)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
