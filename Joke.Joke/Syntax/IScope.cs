using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public interface IScope
    {
        bool TryAdd(INamed named);

        IAny FullLookup(INamed named);
    }
}
