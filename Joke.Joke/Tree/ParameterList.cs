using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ParameterList : Collection<IParameter>
    {
        public ParameterList(TokenSpan span, IReadOnlyList<IParameter> items)
            : base(span, items)
        {
        }
    }
}
