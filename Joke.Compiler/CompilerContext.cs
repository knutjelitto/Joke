using Joke.Outside;

namespace Joke.Compiler
{
    public class CompilerContext
    {
        public CompilerContext(DirRef packages)
        {
            Packages = packages;
        }

        public DirRef Packages { get; }

        public DirRef? FindPackage(string packageName)
        {
            var packageDir = Packages.Dir(packageName);
            if (!packageDir.Exists)
            {
                return null;
            }
            return packageDir;
        }
    }
}
