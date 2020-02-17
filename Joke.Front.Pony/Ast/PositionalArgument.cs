using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class PositionalArgument : Argument
    {
        public PositionalArgument(TSpan span, Expression value)
            : base(span)
        {
            Value = value;
        }

        public Expression Value { get; }
    }
}
