using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public class Package
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
        public LookupList<string, Unit> Units { get; } = new LookupList<string, Unit>();
    }
}
