using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Call : Postfix
    {
        public Call(ISpan span, Expression postfixed, IReadOnlyList<Argument> positional, IReadOnlyList<Argument> named, bool partial)
            : base(span, postfixed)
        {
            Positional = positional;
            Named = named;
            Partial = partial;
        }

        public IReadOnlyList<Argument> Positional { get; }
        public IReadOnlyList<Argument> Named { get; }
        public bool Partial { get; }
    }
}
