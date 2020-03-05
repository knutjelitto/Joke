using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ValueParameterList : Collection<ValueParameter>
    {
        public ValueParameterList(TokenSpan span, IReadOnlyList<ValueParameter> items)
            : base(span, items)
        {
        }
    }
}
