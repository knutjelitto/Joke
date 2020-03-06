using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Try : IExpression
    {
        public Try(TokenSpan span, IExpression body, Else? @else, Then? then)
        {
            Span = span;
            Body = body;
            Else = @else;
            Then = then;
        }

        public TokenSpan Span { get; }
        public IExpression Body { get; }
        public Else? Else { get; }
        public Then? Then { get; }
    }
}
