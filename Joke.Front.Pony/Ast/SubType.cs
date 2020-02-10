using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class SubType : Expression
    {
        public SubType(ISpan span, Type sub, Type type)
            : base(span)
        {
            Sub = sub;
            Type = type;
        }

        public Type Sub { get; }
        public Type Type { get; }
    }
}
