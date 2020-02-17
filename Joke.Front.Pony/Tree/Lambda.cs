using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Lambda : Expression
    {
        public Lambda(TSpan span,
            bool bare,
            Annotations? annotations,
            Cap? receiverCap,
            Identifier? name,
            TypeParameters? typeParameters,
            LambdaParameters parameters,
            LambdaCaptures? captures,
            Type? returnType,
            Partial? partial,
            Expression? body,
            Cap? referenceCap)
            : base(span)
        {
            Bare = bare;
            Annotations = annotations;
            ReceiverCap = receiverCap;
            Name = name;
            TypeParameters = typeParameters;
            Parameters = parameters;
            Captures = captures;
            ReturnType = returnType;
            Partial = partial;
            Body = body;
            ReferenceCap = referenceCap;
        }

        public bool Bare { get; }
        public Annotations? Annotations { get; }
        public Cap? ReceiverCap { get; }
        public Identifier? Name { get; }
        public TypeParameters? TypeParameters { get; }
        public LambdaParameters Parameters { get; }
        public LambdaCaptures? Captures { get; }
        public Type? ReturnType { get; }
        public Partial? Partial { get; }
        public Expression? Body { get; }
        public Cap? ReferenceCap { get; }
    }
}
