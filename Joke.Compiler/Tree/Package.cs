using System.Collections.Generic;

namespace Joke.Compiler.Tree
{
    public class Package
    {
        public Package(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public List<Dependency> Dependencies { get; } = new List<Dependency>();
        public List<PackageMember> Members { get; } = new List<PackageMember>();
    }
}
