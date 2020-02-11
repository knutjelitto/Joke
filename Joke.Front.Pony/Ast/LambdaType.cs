using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class LambdaType : Type
    {
        public LambdaType(
            ISpan span,
            bool bare,
            Capability? capability1,
            Identifier? identifier,
            IReadOnlyList<TypeParameter> parameters,
            IReadOnlyList<Type> argumentTypes,
            Type? returnType,
            Boolean partial,
            Capability? capability2)
            : base(span)
        {
            Bare = bare;
            Capability1 = capability1;
            Identifier = identifier;
            Parameters = parameters;
            ArgumentTypes = argumentTypes;
            ReturnType = returnType;
            Partial = partial;
            Capability2 = capability2;
        }

        public bool Bare { get; }
        public Capability? Capability1 { get; }
        public Identifier? Identifier { get; }
        public IReadOnlyList<TypeParameter> Parameters { get; }
        public IReadOnlyList<Type> ArgumentTypes { get; }
        public Type? ReturnType { get; }
        public Boolean Partial { get; }
        public Capability? Capability2 { get; }
    }
}
