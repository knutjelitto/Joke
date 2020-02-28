using System.Collections.Generic;

using Joke.Front.Pony.ParseTree;

namespace Joke.Compiler.Tree
{
    public class ForeignParameters
    {
        public ForeignParameters(PtParameters source, IReadOnlyList<Parameter> items, bool ellipsis)
        {
            Source = source;
            Items = items;
            Ellipsis = ellipsis;
        }

        public PtParameters Source { get; }
        public IReadOnlyList<Parameter> Items { get; }
        public bool Ellipsis { get; }
    }
}
