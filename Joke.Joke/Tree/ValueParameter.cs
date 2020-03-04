using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ValueParameter
    {
        public ValueParameter(TokenSpan span, Identifier name, IType type, IExpression? @default)
        {
            Span = span;
            Name = name;
            Default = @default;
        }

        public TokenSpan Span { get; }
        public Identifier Name { get; }
        public IExpression? Default { get; }
    }
}
