using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Match : IExpression
    {
        public Match(TokenSpan span, IExpression value, IReadOnlyList<Case> cases, Else? @else)
        {
            Span = span;
            Value = value;
            Cases = cases;
            Else = @else;
        }

        public TokenSpan Span { get; }
        public IExpression Value { get; }
        public IReadOnlyList<Case> Cases { get; }
        public Else? Else { get; }
    }
}
