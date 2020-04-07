using System;

using Joke.Joke.Tree;

namespace Joke.Joke.Syntax
{
    public class ClassScope : IScope
    {
        public ClassScope(IScope outer)
        {
            Scope = outer;
        }

        public IScope Scope { get; }

        public bool TryAdd(INamed named)
        {
            throw new NotImplementedException();
        }

        public IAny FullLookup(INamed named)
        {
            throw new NotImplementedException();
        }
    }
}
