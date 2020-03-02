using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public class Package : IPackageMember
    {
        public Package(DirRef packageDir, string name, Package? builtin = null)
        {
            PackageDir = packageDir;
            Name = name;
            Builtin = builtin;
        }

        public DirRef PackageDir { get; }
        public string Name { get; }
        public Package? Builtin { get; }
        public LookupList<string, IPackageMember> Members { get; } = new LookupList<string, IPackageMember>();
        public LookupList<string, Unit> Units { get; } = new LookupList<string, Unit>();
    }
}
