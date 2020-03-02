using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class UsePackage : IUse
    {
        public UsePackage(PtUseUri source, string? alias, string name, Package package)
        {
            Source = source;
            Alias = alias;
            Name = name;
            Package = package;
        }

        public PtUseUri Source { get; }
        public string? Alias { get; }
        public string Name { get; }
        public Package Package { get; }
    }
}
