using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Reference : IExpression
    {
        public Reference(TokenSpan span, Identifier name)
        {
            Span = span;
            Name = name;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
    }
}
