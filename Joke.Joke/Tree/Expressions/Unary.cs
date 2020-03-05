using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Unary : IExpression
    {
        public Unary(TokenSpan span, UnaryOp op, IExpression operand)
        {
            Span = span;
            Op = op;
            Operand = operand;
        }

        public TokenSpan Span { get; }
        public UnaryOp Op { get; }
        public IExpression Operand { get; }
    }
}
