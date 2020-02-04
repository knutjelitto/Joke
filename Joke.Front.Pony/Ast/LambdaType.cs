using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class LambdaType : Type
    {
        public LambdaType(
            ISpan span, 
            Capability? capability1,
            Identifier? identifier,
            IReadOnlyList<TypeParameter> parameters,
            IReadOnlyList<Type> argumentTypes,
            Type? returnType,
            Capability? capability2)
            : base(span)
        {
            Capability1 = capability1;
            Identifier = identifier;
            Parameters = parameters;
            ArgumentTypes = argumentTypes;
            ReturnType = returnType;
            Capability2 = capability2;
        }

        public Capability? Capability1 { get; }
        public Identifier? Identifier { get; }
        public IReadOnlyList<TypeParameter> Parameters { get; }
        public IReadOnlyList<Type> ArgumentTypes { get; }
        public Type? ReturnType { get; }
        public Capability? Capability2 { get; }
    }
}
