using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public abstract class Node
    {
        protected Node(PonyTokenSpan span)
        {
            Span = span;
        }

        public PonyTokenSpan Span { get; }
    }
}
