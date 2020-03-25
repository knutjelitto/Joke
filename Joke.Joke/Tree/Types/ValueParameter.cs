using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ValueParameter : IParameter
    {
        public ValueParameter(TokenSpan span, Identifier name, IType type, IExpression? value)
        {
            Span = span;
            Name = name;
            Type = type;
            Default = value;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IType Type { get; }
        public IExpression? Default { get; }
    }
}
