using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class UsePackage : IUse
    {
        public UsePackage(PtUseUri source, string? alias, string path, Package package)
        {
            Source = source;
            Alias = alias;
            Path = path;
            Package = package;
        }

        public PtUseUri Source { get; }
        public string? Alias { get; }
        public string Path { get; }
        public Package Package { get; }
    }
}
