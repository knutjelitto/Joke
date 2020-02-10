using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Is : Expression
    {
        public Is(ISpan span, Expression term1, Expression term2)
            : base(span)
        {
            Term1 = term1;
            Term2 = term2;
        }

        public Expression Term1 { get; }
        public Expression Term2 { get; }
    }
}
