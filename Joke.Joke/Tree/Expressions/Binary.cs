using Joke.Joke.Decoding;
using System.Collections.Generic;

namespace Joke.Joke.Tree
{
    public class Binary : IExpression
    {
        public Binary(TokenSpan span, BinaryOp op, IReadOnlyList<IExpression> operands)
        {
            Span = span;
            Op = op;
            Operands = operands;
        }

        public TokenSpan Span { get; }
        public BinaryOp Op { get; }
        public IReadOnlyList<IExpression> Operands { get; }
    }
}
