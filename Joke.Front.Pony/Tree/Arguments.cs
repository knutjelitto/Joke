using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Arguments : Node
    {
        public Arguments(TSpan span, IReadOnlyList<Argument> arguments)
            : base(span)
        {
            Items = arguments;
        }

        public IReadOnlyList<Argument> Items { get; }
    }
}
