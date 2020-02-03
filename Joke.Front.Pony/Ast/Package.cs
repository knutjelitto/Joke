using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Package
    {
        public Package(IEnumerable<Unit> units)
        {
            Units = units.ToArray();
        }

        public IReadOnlyList<Unit> Units { get; }
    }
}
