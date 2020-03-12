using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ParameterList : Collection<IParameter>, IAny
    {
        public ParameterList(TokenSpan span, IReadOnlyList<IParameter> items)
            : base(items)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
