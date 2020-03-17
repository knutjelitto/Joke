using Joke.Joke.Tools;

namespace Joke.Joke.Syntax
{
    public class Class : DistinctList<Tree.Identifier, Member>, ISourcedName
    {
        public Class(Unit unit, Tree.ClassType source)
        {
            Unit = unit;
            Source = source;
        }

        public Unit Unit { get; }
        public Tree.INamedMember Source { get; }
        public Tree.Identifier Name => Source.Name;
    }
}
