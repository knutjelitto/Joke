using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public interface IContainer
    {
        LookupList<string, IClassMember> Members { get; }
    }
}
