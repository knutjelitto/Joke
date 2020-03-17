using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public abstract class Member : ISourcedName
    {
        public abstract Tree.Identifier Name { get; }
        public abstract INamedMember Source { get; }
    }
}
