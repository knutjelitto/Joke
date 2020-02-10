using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class NominalType : Type
    {
        public NominalType(ISpan span, Identifier identifier, TypeArguments? arguments, Capability? capability, NominalKind kind)
            : base(span)
        {
            Identifier = identifier;
            Arguments = arguments;
            Capability = capability;
            Kind = kind;
        }

        public Identifier Identifier { get; }
        public TypeArguments? Arguments { get; }
        public Capability? Capability { get; }
        public NominalKind Kind { get; }
    }
}
