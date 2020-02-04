using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class TypeParameter : Base
    {
        public TypeParameter(ISpan span, Identifier identifier, Type? constraint, Type? @default)
            : base(span)
        {
            Identifier = identifier;
            Constraint = constraint;
            Default = @default;
        }

        public Identifier Identifier { get; }
        public Type? Constraint { get; }
        public Type? Default { get; }
    }
}
