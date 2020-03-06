using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class When : IExpression
    {
        public When(TokenSpan span, IExpression condition)
        {
            Span = span;
            Condition = condition;
        }

        public TokenSpan Span { get; }
        public IExpression Condition { get; }
    }
}
