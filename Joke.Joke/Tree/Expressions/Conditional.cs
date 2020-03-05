using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Conditional : IExpression
    {
        public Conditional(TokenSpan span, IExpression condition, IExpression body)
        {
            Span = span;
            Condition = condition;
            Body = body;
        }
        public TokenSpan Span { get; }
        public IExpression Condition { get; }
        public IExpression Body { get; }
    }
}
