using System;

using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public class PackageScope : IScope
    {
        public PackageScope(Package package)
        {
            Package = package;
        }

        public Package Package { get; }

        public bool TryAdd(INamed named)
        {
            return Package.Members.TryAdd(named);
        }

        public IAny FullLookup(INamed named)
        {
            throw new NotImplementedException();
        }
    }
}
