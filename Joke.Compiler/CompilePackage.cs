using System.Collections.Generic;

using Joke.Front.Err;
using Joke.Outside;

namespace Joke.Compiler
{
    public class CompilePackage
    {
        public CompilePackage(Compilation context, DirRef packageDir, string name)
        {
            Compilation = context;
            PackageDir = packageDir;
            Name = name;
            Loaded = false;
        }

        public Compilation Compilation { get; }
        public IndentWriter Logger => Compilation.Logger;
        public ErrorAccu Errors => Compilation.Errors;
        public DirRef PackageDir { get; }
        public string Name { get; }
        public bool Loaded { get; private set; }

        public List<CompileFile> Units { get; } = new List<CompileFile>();

        public void Load()
        {
            if (!Loaded)
            {
                foreach (var unitFile in PackageDir.Files("*.pony"))
                {
                    var unit = new CompileFile(this, unitFile);
                    Units.Add(unit);
                }

                Loaded = true;
            }
        }
    }
}
