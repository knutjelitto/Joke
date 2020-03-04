using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class CompileIntrinsic : IExpression
    {
        public CompileIntrinsic(TokenSpan span, IExpression? value)
        {
            Span = span;
            Value = value;
        }

        public TokenSpan Span { get; }
        public IExpression? Value { get; }
    }
}
