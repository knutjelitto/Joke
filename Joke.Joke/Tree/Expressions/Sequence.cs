using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Sequence : Collection<IExpression>, IExpression
    {
        public Sequence(TokenSpan span, IReadOnlyList<IExpression> items)
            : base(span, items)
        {
        }
    }
}
