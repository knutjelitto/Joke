using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class TypeName : Type
    {
        public TypeName(ISpan span, Capability capability, Identifier id, IEnumerable<Type> arguments)
            : base(span, capability)
        {
            Id = id;
            Arguments = arguments.ToArray();
        }

        public Identifier Id { get; }
        public IReadOnlyList<Type> Arguments { get; }
    }
}
