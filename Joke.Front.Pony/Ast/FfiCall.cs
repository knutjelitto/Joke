using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class FfiCall : Expression
    {
        public FfiCall(ISpan span, ExternIdentifier name, IReadOnlyList<Argument> positional, IReadOnlyList<Argument> named, bool partial)
            : base(span)
        {
            Name = name;
            Positional = positional;
            Named = named;
            Partial = partial;
        }

        public ExternIdentifier Name { get; }
        public IReadOnlyList<Argument> Positional { get; }
        public IReadOnlyList<Argument> Named { get; }
        public bool Partial { get; }
    }
}
