using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class As : Expression
    {
        public As(ISpan span, Expression term, Type type)
            : base(span)
        {
            Term = term;
            Type = type;
        }

        public Expression Term { get; }
        public Type Type { get; }
    }
}
