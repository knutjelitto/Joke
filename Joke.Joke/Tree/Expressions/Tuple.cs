using Joke.Joke.Decoding;
using System.Collections.Generic;

namespace Joke.Joke.Tree
{
    public class Tuple : IExpression
    {
        public Tuple(TokenSpan span, IReadOnlyList<IExpression> expressions)
        {
            Span = span;
            Expressions = expressions;
        }

        public TokenSpan Span { get; }
        public IReadOnlyList<IExpression> Expressions { get; }
    }
}
