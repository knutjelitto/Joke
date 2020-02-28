using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class UseForeignFunction : IUse
    {
        public UseForeignFunction(PtUseFfi source, ForeignFunctionDeclaration function)
        {
            Source = source;
            Function = function;
        }

        public PtUseFfi Source { get; }
        public ForeignFunctionDeclaration Function { get; }
    }
}
