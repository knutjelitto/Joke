using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class Parameter
    {
        public Parameter(PtRegularParameter source, string name, IType type, IExpression? defaultValue)
        {
            Source = source;
            Name = name;
            Type = type;
            DefaultValue = defaultValue;
        }

        public PtRegularParameter Source { get; }
        public string Name { get; }
        public IType Type { get; }
        public IExpression? DefaultValue { get; }
    }
}
