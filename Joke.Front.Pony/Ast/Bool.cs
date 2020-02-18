using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Bool : Literal
    {
        public Bool(TSpan span, bool value)
            : base(span)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
