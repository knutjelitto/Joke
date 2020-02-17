using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Bool : Expression
    {
        public Bool(TSpan span, bool value)
            : base(span)
        {
            Value = value;
        }

        public bool Value { get; }
    }
}
