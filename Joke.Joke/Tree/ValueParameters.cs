using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ValueParameters : Collection<ValueParameter>
    {
        public ValueParameters(TokenSpan span, IReadOnlyList<ValueParameter> items)
            : base(span, items)
        {
        }
    }
}
