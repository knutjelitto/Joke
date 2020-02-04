using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class NominalType : Type
    {
        public NominalType(ISpan span, Identifier identifier, IReadOnlyList<Type> arguments, Capability? capability)
            : base(span)
        {
            Identifier = identifier;
            Arguments = arguments;
            Capability = capability;
        }

        public Identifier Identifier { get; }
        public IReadOnlyList<Type> Arguments { get; }
        public Capability? Capability { get; }
    }
}
