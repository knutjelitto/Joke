using Joke.Front.Err;
using Joke.Outside;
using System.Collections.Generic;

namespace Joke.Compiler
{
    public class Compilation
    {
        private Dictionary<string, Package> packageIndex = new Dictionary<string, Package>();
        public List<Package> Packages { get; } = new List<Package>();


        public Compilation(CompilerContext context, DirRef packageDir, string name, bool isBuiltin = false)
        {
            Context = context;
            PackageDir = packageDir;
            Name = name;
            IsBuiltin = isBuiltin;
        }

        public CompilerContext Context { get; }
        public ErrorAccu Errors => Context.Errors;
        public IndentWriter Logger => Context.Logger;
        public DirRef PackageDir { get; }
        public string Name { get; }
        public bool IsBuiltin { get; }

        public void Load()
        {
            if (!IsBuiltin)
            {
                var builtin = UsePackage("builtin");
            }

            var package = new Package(this, PackageDir, Name);
            package.Load();

            var i = 0;
            while (i < Packages.Count)
            {
                Packages[i].Load();
                i += 1;
            }
        }


        public Package UsePackage(string packageName)
        {
            var packageDir = Context.FindPackageDir(packageName);

            if (!packageIndex.TryGetValue(packageDir, out var package))
            {
                package = new Package(this, packageDir, packageName);
                packageIndex.Add(packageDir, package);
                Packages.Add(package);
            }

            return package;
        }

    }
}
