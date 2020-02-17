using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class As : Expression
    {
        public As(TSpan span, Expression value, Type type)
            : base(span)
        {
            Value = value;
            Type = type;
        }

        public Expression Value { get; }
        public Type Type { get; }
    }
}
