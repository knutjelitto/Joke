using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public interface ISourcedName
    {
        INamedMember Source { get; }
    }
}
