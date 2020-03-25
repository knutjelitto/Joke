namespace Joke.Joke.Tree
{
    public interface IVisited
    {
        void Accept(IVisitor visitor);
    }
}
