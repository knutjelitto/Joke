using Joke.Front.Pony.Lexing;
using System.Collections.Generic;

namespace Joke.Front.Pony.ParseTree
{
    public class PtArguments : PtNode
    {
        public PtArguments(PonyTokenSpan span, IReadOnlyList<PtArgument> arguments)
            : base(span)
        {
            Items = arguments;
        }

        public IReadOnlyList<PtArgument> Items { get; }
    }
}
