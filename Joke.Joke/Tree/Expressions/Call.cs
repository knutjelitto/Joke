using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Call : IExpression
    {
        public Call(TokenSpan span, IExpression expression, ArgumentList arguments, Throws? throws)
        {
            Span = span;
            Expression = expression;
            Arguments = arguments;
            Throws = throws;
        }

        public TokenSpan Span { get; }
        public IExpression Expression { get; }
        public ArgumentList Arguments { get; }
        public Throws? Throws { get; }
    }
}
