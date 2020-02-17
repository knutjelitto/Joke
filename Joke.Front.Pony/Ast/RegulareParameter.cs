using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class RegularParameter : Parameter
    {
        public RegularParameter(TSpan span, Identifier name, Type type, Expression? value)
            : base(span)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public Identifier Name { get; }
        public Type Type { get; }
        public Expression? Value { get; }
    }
}
