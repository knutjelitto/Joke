namespace Joke.Front.Joke.Ast
{
    public class Node
    {
        public Node(SourceSpan span)
        {
            Span = span;
        }

        public SourceSpan Span { get; }
    }
}
