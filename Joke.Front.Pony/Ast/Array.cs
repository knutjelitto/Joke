using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Array : Expression
    {
        public Array(ISpan span, Type? type, Expression? elements)
            : base(span)
        {
            Type = type;
            Elements = elements;
        }

        public Type? Type { get; }
        public Expression? Elements { get; }
    }
}
