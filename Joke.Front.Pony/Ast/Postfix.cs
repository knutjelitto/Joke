using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Postfix : Expression
    {
        public Postfix(ISpan span, Expression postfixed)
            : base(span)
        {
            Postfixed = postfixed;
        }

        public Expression Postfixed { get; }
    }
}
