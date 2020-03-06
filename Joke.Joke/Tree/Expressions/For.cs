using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class For : IExpression
    {
        public For(TokenSpan span, INamePattern names, IExpression values, IExpression body, Else? @else)
        {
            Span = span;
            Names = names;
            Values = values;
            Body = body;
            Else = @else;
        }

        public TokenSpan Span { get; }
        public INamePattern Names { get; }
        public IExpression Values { get; }
        public IExpression Body { get; }
        public Else? Else { get; }
    }
}
