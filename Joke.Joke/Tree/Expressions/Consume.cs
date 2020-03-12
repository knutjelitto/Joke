using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Consume : IExpression
    {
        public Consume(TokenSpan span, Cap? cap, IExpression expression)
        {
            Span = span;
            Cap = cap;
            Expression = expression;
        }

        public TokenSpan Span { get; }
        public Cap? Cap { get; }
        public IExpression Expression { get; }
    }
}
