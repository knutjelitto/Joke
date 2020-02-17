using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class LambdaType : Type
    {
        public LambdaType(TSpan span,
            bool bare,
            Cap? receiverCap,
            Identifier? name,
            TypeParameters? typeParameters,
            LambdaTypeParameters parameters,
            Type? returnType,
            Partial? partial,
            Cap? referenceCap,
            EphemAlias ea)
            : base(span)
        {
            Bare = bare;
            ReceiverCap = receiverCap;
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            ReturnType = returnType;
            Partial = partial;
            ReferenceCap = referenceCap;
            Ea = ea;
        }

        public bool Bare { get; }
        public Cap? ReceiverCap { get; }
        public Identifier? Name { get; }
        public TypeParameters? TypeParameters { get; }
        public LambdaTypeParameters Parameters { get; }
        public Type? ReturnType { get; }
        public Partial? Partial { get; }
        public Cap? ReferenceCap { get; }
        public EphemAlias Ea { get; }
    }
}
