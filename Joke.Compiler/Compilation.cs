using Joke.Front.Err;
using Joke.Outside;
using System.Collections.Generic;

namespace Joke.Compiler
{
    public class Compilation
    {
        private Dictionary<string, CompilePackage> packageIndex = new Dictionary<string, CompilePackage>();
        public List<CompilePackage> Packages { get; } = new List<CompilePackage>();


        public Compilation(CompileContext context, DirRef packageDir, string name, bool isBuiltin = false)
        {
            Context = context;
            PackageDir = packageDir;
            Name = name;
            IsBuiltin = isBuiltin;
        }

        public CompileContext Context { get; }
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

            var package = new CompilePackage(this, PackageDir, Name);
            package.Load();

            var i = 0;
            while (i < Packages.Count)
            {
                Packages[i].Load();
                i += 1;
            }
        }


        public CompilePackage UsePackage(string packageName)
        {
            var packageDir = Context.FindPackageDir(packageName);

            if (!packageIndex.TryGetValue(packageDir, out var package))
            {
                package = new CompilePackage(this, packageDir, packageName);
                packageIndex.Add(packageDir, package);
                Packages.Add(package);
            }

            return package;
        }

    }
}
