using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    class Generic : IExpression
    {
        public Generic(TokenSpan span, IExpression expression, TypeList typeArguments)
        {
            Span = span;
            Expression = expression;
            Arguments = typeArguments;
        }

        public TokenSpan Span { get; }
        public IExpression Expression { get; }
        public TypeList Arguments { get; }
    }
}
