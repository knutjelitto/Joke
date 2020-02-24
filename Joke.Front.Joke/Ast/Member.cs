namespace Joke.Front.Joke.Ast
{
    public abstract class Member : Node
    {
        public Member(SourceSpan span)
            : base(span)
        {
        }
    }
}
