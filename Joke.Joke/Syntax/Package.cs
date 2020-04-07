using System.Collections.Generic;

using Joke.Joke.Err;
using Joke.Joke.Tools;
using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public class Package
    {
        public Package(Identifier? alias, IReadOnlyList<Unit> units)
        {
            Alias = alias;
            Units = units;
            Packages = new List<Package>();
            Members = new NamedList<INamedMember>();
            Errors = new Errors();
            Scope = new PackageScope(this);
        }

        public Identifier? Alias { get; }
        public IReadOnlyList<Unit> Units { get; }
        public List<Package> Packages { get; }
        public NamedList<INamedMember> Members { get; }
        public Errors Errors { get; }
        public IScope Scope { get; }

        public void Build()
        {
            new PackageBuilder(this).Build();
        }
    }
}
