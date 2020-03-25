using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ArgumentList : Collection<IExpression>
    {
        public ArgumentList(TokenSpan span, IReadOnlyList<IExpression> items)
            : base(items)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
