using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class SubType : Expression
    {
        public SubType(TSpan span, Type sub, Type super)
            : base(span)
        {
            Sub = sub;
            Super = super;
        }

        public Type Sub { get; }
        public Type Super { get; }
    }
}
