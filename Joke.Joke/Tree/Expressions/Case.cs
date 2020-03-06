using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Case : IExpression
    {
        public Case(TokenSpan span, IExpression? pattern, When? when, Body? body)
        {
            Span = span;
            Pattern = pattern;
            When = when;
            Body = body;
        }

        public TokenSpan Span { get; }
        public IExpression? Pattern { get; }
        public When? When { get; }
        public Body? Body { get; }
    }
}
