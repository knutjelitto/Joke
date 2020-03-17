using Joke.Joke.Tools;

namespace Joke.Joke.Syntax
{
    public class Unit : DistinctList<Tree.Identifier, Class>
    {
        public Unit(Package package, Tree.Unit source)
        {
            Package = package;
            Source = source;
        }

        public Package Package { get; }
        public Tree.Unit Source { get; }
    }
}
