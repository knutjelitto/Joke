using Joke.Compiler.Compile;
using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public class Package : IPackageMember
    {
        public Package(CompileContext context, DirRef packageDir, string name, Package? builtin = null)
        {
            Context = context;
            PackageDir = packageDir;
            Name = name;
            Builtin = builtin;
        }

        public CompileContext Context { get; }
        public DirRef PackageDir { get; }
        public string Name { get; }
        public Package? Builtin { get; }
        public LookupList<string, IPackageMember> Members { get; } = new LookupList<string, IPackageMember>();
        public LookupList<string, Unit> Units { get; } = new LookupList<string, Unit>();
    }
}
