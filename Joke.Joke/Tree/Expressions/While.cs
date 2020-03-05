using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class While : IExpression
    {
        public While(TokenSpan span, IExpression condition, IExpression body, Else? @else)
        {
            Span = span;
            Condition = condition;
            Body = body;
            Else = @else;
        }

        public TokenSpan Span { get; }
        public IExpression Condition { get; }
        public IExpression Body { get; }
        public Else? Else { get; }
    }
}
