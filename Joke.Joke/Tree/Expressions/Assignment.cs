using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Assignment : IExpression
    {
        public Assignment(TokenSpan span, IExpression left, IExpression right)
        {
            Span = span;
            Left = left;
            Right = right;
        }

        public TokenSpan Span { get; }
        public IExpression Left { get; }
        public IExpression Right { get; }
    }
}
