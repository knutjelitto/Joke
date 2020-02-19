using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public abstract class Node
    {
        protected Node(TokenSpan span)
        {
            Span = span;
        }

        public TokenSpan Span { get; }
    }
}
