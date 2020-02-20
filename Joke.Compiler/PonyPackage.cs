using Joke.Outside;

namespace Joke.Compiler
{
    public class PonyPackage
    {
        public PonyPackage(DirRef packageDir)
        {
            PackageDir = packageDir;
        }

        public DirRef PackageDir { get; }
    }
}
