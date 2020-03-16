using System.Diagnostics;
using System.Linq;

using Joke.Joke.Err;

namespace Joke.Joke.Syntax
{
    public class Unit
    {
        public Unit(Package package, Tree.Unit source)
        {
            Package = package;
            Source = source;
            Members = new TopMemberlist();
        }

        public Package Package { get; }
        public Errors Errors => Package.Errors;
        public Tree.Unit Source { get; }

        public TopMemberlist Members { get; }
    }
}
