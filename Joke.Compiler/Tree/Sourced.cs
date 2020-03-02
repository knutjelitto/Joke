using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public interface ISourced
    {
        public PtNode Source { get; }
    }
}
