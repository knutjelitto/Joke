using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class NameCapture : ICapture, INamed
    {
        public NameCapture(TokenSpan span, Identifier name, IType? type, IExpression? value)
        {
            Span = span;
            Name = name;
            Type = type;
            Value = value;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IType? Type { get; }
        public IExpression? Value { get; }
    }
}
