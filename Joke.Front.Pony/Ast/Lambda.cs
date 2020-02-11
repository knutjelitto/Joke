using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Lambda : Expression
    {
        public Lambda(ISpan span, 
            bool bare,
            Capability? receiverCap,
            Identifier? name,
            TypeParameters typeParameters,
            IReadOnlyList<LambdaParameter> lambdaParameters,
            IReadOnlyList<LambdaCapture> lambdaCaptures,
            Type? @return,
            Boolean partial,
            Expression body,
            Capability? referenceCap)
            : base(span)
        {
            Bare = bare;
            ReceiverCap = receiverCap;
            Name = name;
            TypeParameters = typeParameters;
            LambdaParameters = lambdaParameters;
            LambdaCaptures = lambdaCaptures;
            Return = @return;
            Partial = partial;
            Body = body;
            ReferenceCap = referenceCap;
        }

        public Boolean Bare { get; }
        public Capability? ReceiverCap { get; }
        public Identifier? Name { get; }
        public TypeParameters TypeParameters { get; }
        public IReadOnlyList<LambdaParameter> LambdaParameters { get; }
        public IReadOnlyList<LambdaCapture> LambdaCaptures { get; }
        public Type? Return { get; }
        public Boolean Partial { get; }
        public Expression Body { get; }
        public Capability? ReferenceCap { get; }
    }
}
