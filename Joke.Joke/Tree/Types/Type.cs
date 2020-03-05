using Joke.Joke.Decoding;

namespace Joke.Joke.Tree
{
    public class Type : IType
    {
        public Type(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
