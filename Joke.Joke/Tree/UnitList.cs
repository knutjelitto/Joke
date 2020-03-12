using System.Collections.Generic;

namespace Joke.Joke.Tree
{
    public class UnitList : Collection<CompilationUnit>
    {
        public UnitList(IReadOnlyList<CompilationUnit> units)
            : base(units)
        { 
        }
    }
}
