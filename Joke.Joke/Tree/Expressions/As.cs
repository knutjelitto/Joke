using System.Collections.Generic;

using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class As : IExpression
    {
        public As(TokenSpan span, IExpression expression, IReadOnlyList<IType> types)
        {
            Span = span;
            Expression = expression;
            Types = types;
        }

        public TokenSpan Span { get; }
        public IExpression Expression { get; }
        public IReadOnlyList<IType> Types { get; }
    }
}
