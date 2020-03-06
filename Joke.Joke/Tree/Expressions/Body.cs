using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Body : IExpression
    {
        public Body(TokenSpan span, IExpression expression)
        {
            Span = span;
            Expression = expression;
        }

        public TokenSpan Span { get; }
        public IExpression Expression { get; }
    }
}
