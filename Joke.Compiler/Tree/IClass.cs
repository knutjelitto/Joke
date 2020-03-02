using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public interface IClass : IPackageMember, IType
    {
        LookupList<string, IClassMember> Members { get; }
    }
}
