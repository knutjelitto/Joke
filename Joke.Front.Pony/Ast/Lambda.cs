using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Lambda : Expression
    {
        public Lambda(ISpan span, 
            bool bare,
            Capability? recCap,
            Identifier? name,
            IReadOnlyList<TypeParameter> typeParameters,
            IReadOnlyList<LambdaParameter> lambdaParameters,
            IReadOnlyList<LambdaCapture> lambdaCaptures,
            Type? @return,
            Boolean partial,
            Expression body,
            Capability? refCap)
            : base(span)
        {
            Bare = bare;
            RecCap = recCap;
            Name = name;
            TypeParameters = typeParameters;
            LambdaParameters = lambdaParameters;
            LambdaCaptures = lambdaCaptures;
            Return = @return;
            Partial = partial;
            Body = body;
            RefCap = refCap;
        }

        public Boolean Bare { get; }
        public Capability? RecCap { get; }
        public Identifier? Name { get; }
        public IReadOnlyList<TypeParameter> TypeParameters { get; }
        public IReadOnlyList<LambdaParameter> LambdaParameters { get; }
        public IReadOnlyList<LambdaCapture> LambdaCaptures { get; }
        public Type? Return { get; }
        public Boolean Partial { get; }
        public Expression Body { get; }
        public Capability? RefCap { get; }
    }
}
