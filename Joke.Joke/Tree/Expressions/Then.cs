using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Then : IExpression
    {
        public Then(TokenSpan span, IExpression body)
        {
            Span = span;
            Body = body;
        }

        public TokenSpan Span { get; }
        public IExpression Body { get; }
    }
}
