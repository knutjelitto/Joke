using System;
using System.Collections.Generic;

using Joke.Front.Err;
using Joke.Outside;

namespace Joke.Compiler
{
    public class CompilerContext
    {
        private Dictionary<string, PonyPackage> loadedPackages = new Dictionary<string, PonyPackage>();

        public CompilerContext(ErrorAccu errors, DirRef packages)
        {
            Errors = errors;
            Packages = packages;
            Logger = new IndentWriter(Console.Out, " .. ");
        }

        public ErrorAccu Errors { get; }
        public DirRef Packages { get; }
        public IndentWriter Logger { get; }

        public DirRef FindPackage(string packageName)
        {
            var packageDir = Packages.Dir(packageName);
#if false
            if (!packageDir.Exists)
            {
                return null;
            }
#endif
            return packageDir;
        }

        public PonyPackage LoadPackage(string packageName)
        {
            var packageDir = FindPackage(packageName);

            if (!loadedPackages.TryGetValue(packageDir, out var package))
            {
                using (Logger.Indent())
                {
                    package = new PonyPackage(this, packageDir, packageName == "builtin");
                    loadedPackages.Add(packageDir, package);
                }
            }

            return package;
        }
    }
}
