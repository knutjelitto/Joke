using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public class Consume : Expression
    {
        public Consume(TokenSpan span, Cap? cap, Expression expression)
            : base(span)
        {
            Cap = cap;
            Expression = expression;
        }

        public Cap? Cap { get; }
        public Expression Expression { get; }
    }
}
