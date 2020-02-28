using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class AnyExpression : IExpression
    {
        public AnyExpression(PtExpression source)
        {
            Source = source;
        }

        public PtExpression Source { get; }
    }
}
