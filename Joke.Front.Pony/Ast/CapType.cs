using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class CapType : Type
    {
        public CapType(SourceSpan span, Type type, Capability capability)
            : base(span)
        {
            Type = type;
            Capability = capability;
        }

        public Type Type { get; }
        public Capability Capability { get; }
    }
}
