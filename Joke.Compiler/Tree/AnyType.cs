using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class AnyType : IType
    {
        public AnyType(PtType source)
        {
            Source = source;
        }

        public PtType Source { get; }
    }
}
