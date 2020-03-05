using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Call : IExpression
    {
        public Call(TokenSpan span, IExpression expression, ArgumentList arguments)
        {
            Span = span;
            Expression = expression;
            Arguments = arguments;
        }

        public TokenSpan Span { get; }
        public IExpression Expression { get; }
        public ArgumentList Arguments { get; }
    }
}
