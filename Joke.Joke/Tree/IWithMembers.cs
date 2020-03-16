using Joke.Joke.Tools;

namespace Joke.Joke.Tree
{
    public interface IWithMembers<M> where M : class, IMember
    {
        LookupList<Identifier, M> Members { get; }
    }
}
