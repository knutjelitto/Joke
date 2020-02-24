using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class Consume : Expression
    {
        public Consume(PonyTokenSpan span, Cap? cap, Expression expression)
            : base(span)
        {
            Cap = cap;
            Expression = expression;
        }

        public Cap? Cap { get; }
        public Expression Expression { get; }
    }
}
