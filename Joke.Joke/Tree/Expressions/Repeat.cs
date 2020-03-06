using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Repeat : IExpression
    {
        public Repeat(TokenSpan span, IExpression body, IExpression condition, Else? @else)
        {
            Span = span;
            Body = body;
            Condition = condition;
            Else = @else;
        }

        public TokenSpan Span { get; }
        public IExpression Body { get; }
        public IExpression Condition { get; }
        public Else? Else { get; }
    }
}
