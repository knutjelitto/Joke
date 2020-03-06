using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class NamedArgument : IExpression
    {
        public NamedArgument(TokenSpan span, Identifier name, IExpression value)
        {
            Span = span;
            Name = name;
            Value = value;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IExpression Value { get; }
    }
}
