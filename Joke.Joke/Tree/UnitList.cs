using System.Collections.Generic;

namespace Joke.Joke.Tree
{
    public class UnitList : Collection<Unit>
    {
        public UnitList(IReadOnlyList<Unit> units)
            : base(units)
        { 
        }
    }
}
