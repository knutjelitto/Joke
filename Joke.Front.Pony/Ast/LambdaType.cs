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
            Capability? receiverCapability,
            Identifier? identifier,
            TypeParameters parameters,
            IReadOnlyList<Type> argumentTypes,
            Type? returnType,
            Boolean partial,
            Capability? referenceCapbility)
            : base(span)
        {
            Bare = bare;
            ReceiverCap = receiverCapability;
            Identifier = identifier;
            Parameters = parameters;
            ArgumentTypes = argumentTypes;
            ReturnType = returnType;
            Partial = partial;
            ReferenceCap = referenceCapbility;
        }

        public bool Bare { get; }
        public Capability? ReceiverCap { get; }
        public Identifier? Identifier { get; }
        public TypeParameters Parameters { get; }
        public IReadOnlyList<Type> ArgumentTypes { get; }
        public Type? ReturnType { get; }
        public Boolean Partial { get; }
        public Capability? ReferenceCap { get; }
    }
}
