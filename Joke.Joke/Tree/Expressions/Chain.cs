using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Chain : IExpression
    {
        public Chain(TokenSpan span, IExpression expression, Identifier member)
        {
            Span = span;
            Expression = expression;
            Member = member;
        }

        public TokenSpan Span { get; }
        public IExpression Expression { get; }
        public Identifier Member { get; }
    }
}
