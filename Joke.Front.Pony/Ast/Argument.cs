using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Argument : Base
    {
        public Argument(ISpan span, Identifier? name, Expression value)
            : base(span)
        {
            Name = name;
            Value = value;
        }

        public Identifier? Name { get; }
        public Expression Value { get; }
    }
}
