using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class NamedArgument : Argument
    {
        public NamedArgument(TSpan span, Identifier name, Expression value)
            : base(span)
        {
            Name = name;
            Value = value;
        }

        public Identifier Name { get; }
        public Expression Value { get; }
    }
}
