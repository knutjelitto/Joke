using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class ViewType : IType
    {
        public ViewType(TokenSpan span, IType from, IType to)
        {
            Span = span;
            From = from;
            To = to;
        }

        public TokenSpan Span { get; }
        public IType From { get; }
        public IType To { get; }
    }
}
