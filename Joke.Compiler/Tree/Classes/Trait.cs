using Joke.Front.Pony.ParseTree;
using Joke.Outside;

namespace Joke.Compiler.Tree
{
    public class Trait : IClass
    {
        public Trait(PtClass source, Unit unit, string name)
        {
            Source = source;
            Unit = unit;
            Name = name;
        }

        public PtClass Source { get; }
        public Unit Unit { get; }
        public string Name { get; }
        public LookupList<string, IClassMember> Members { get; } = new LookupList<string, IClassMember>();
    }
}
