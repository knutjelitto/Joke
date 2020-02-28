using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class ForeignFunctionDeclaration
    {
        public ForeignFunctionDeclaration(PtUseFfi source, string name, ForeignParameters parameters, IType result)
        {
            Source = source;
            Name = name;
            Parameters = parameters;
            Result = result;
        }

        public PtUseFfi Source { get; }
        public string Name { get; }
        public ForeignParameters Parameters { get; }
        public IType Result { get; }
    }
}
