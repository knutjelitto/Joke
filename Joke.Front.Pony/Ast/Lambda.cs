using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Lambda : Expression
    {
        public Lambda(ISpan span, 
            Capability? recCap,
            Identifier? name,
            IReadOnlyList<TypeParameter> typeParameters,
            IReadOnlyList<LambdaParameter> lambdaParameters,
            IReadOnlyList<LambdaCapture> lambdaCaptures,
            Type? @return,
            bool partial,
            Expression body,
            Capability? refCap)
            : base(span)
        {
        }
    }
}
