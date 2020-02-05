using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Local : Expression
    {
        public Local(ISpan span, Identifier name, Type? type)
            : base(span)
        {
            Name = name;
            Type = type;
        }

        public Identifier Name { get; }
        public Type? Type { get; }
    }
}
