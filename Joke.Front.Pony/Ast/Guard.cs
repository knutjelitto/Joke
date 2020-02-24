using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Guard : Expression
    {
        public Guard(PonyTokenSpan span, Expression expression)
            : base(span)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
