using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ExpressionList : Collection<IExpression>
    {
        public ExpressionList(TokenSpan span, IReadOnlyList<IExpression> items)
            : base(span, items)
        {
        }
    }
}
