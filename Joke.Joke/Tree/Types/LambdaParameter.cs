using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class LambdaParameter : IParameter
    {
        public LambdaParameter(TokenSpan span, Identifier name, IType? type, IExpression? value)
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
