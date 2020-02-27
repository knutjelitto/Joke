using System.Collections.Generic;

using Joke.Front.Err;
using Joke.Outside;

namespace Joke.Compiler
{
    public class Package
    {
        public Package(Compilation context, DirRef packageDir, string name)
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

        public List<File> Units { get; } = new List<File>();

        public void Load()
        {
            if (!Loaded)
            {
                foreach (var unitFile in PackageDir.Files("*.pony"))
                {
                    var unit = new File(this, unitFile);
                    Units.Add(unit);
                }

                Loaded = true;
            }
        }
    }
}
