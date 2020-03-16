using Joke.Joke.Tools;

namespace Joke.Joke.Tree
{
    public interface IWithMembers<M> where M : class, IMember
    {
        DistinctList<Identifier, M> Members { get; }
    }
}
