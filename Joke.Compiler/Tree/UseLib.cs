using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class UseLib : IUse
    { 
        public UseLib(PtUseUri source, string path)
        {
            Source = source;
            Path = path;
        }

        public PtUseUri Source { get; }
        public string Path { get; }
    }
}
