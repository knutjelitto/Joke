using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class CompileError : IExpression
    {
        public CompileError(TokenSpan span, IExpression? value)
        {
            Span = span;
            Value = value;
        }

        public TokenSpan Span { get; }
        public IExpression? Value { get; }
    }
}
