using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class LambdaCaptureName : LambdaCapture
    {
        public LambdaCaptureName(ISpan span, Identifier name, Type? type, Expression? value)
            : base(span)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public Identifier Name { get; }
        public Type? Type { get; }
        public Expression? Value { get; }
    }
}
