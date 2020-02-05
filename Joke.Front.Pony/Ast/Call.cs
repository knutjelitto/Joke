using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Call : Postfix
    {
        public Call(ISpan span, Expression atom, IReadOnlyList<Argument> positional, IReadOnlyList<Argument> named)
            : base(span, atom)
        {
            Positional = positional;
            Named = named;
        }

        public IReadOnlyList<Argument> Positional { get; }
        public IReadOnlyList<Argument> Named { get; }
    }
}
