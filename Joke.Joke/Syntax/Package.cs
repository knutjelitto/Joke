using System.Collections.Generic;

using Joke.Joke.Err;
using Joke.Joke.Tools;
using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public class Package
    {
        public Package(IReadOnlyList<Unit> units)
        {
            Units = units;
            Members = new NamedList<INamedMember>();
            Errors = new Errors();
            Scope = new PackageScope(this);
        }

        public IReadOnlyList<Unit> Units { get; }
        public NamedList<INamedMember> Members { get; }
        public Errors Errors { get; }
        public IScope Scope { get; }

        public void Build()
        {
            new PackageBuilder(this).Build();
        }
    }
}
