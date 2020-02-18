using Joke.Front.Pony.Lex;

namespace Joke.Front.Pony.Ast
{
    public abstract class Node
    {
        protected Node(TSpan span)
        {
            Span = span;
        }

        public TSpan Span { get; }
    }
}
