using System;

using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public class UnitScope : IScope
    {
        public UnitScope(Package package, Unit unit)
        {
            Package = package;
            Unit = unit;
        }

        public Package Package { get; }
        public Unit Unit { get; }

        public IAny FullLookup(INamed named)
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(INamed named)
        {
            throw new NotImplementedException();
        }
    }
}
