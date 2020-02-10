using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Tilde : Expression
    {
        public Tilde(ISpan span, Expression expression, Identifier name)
            : base(span)
        {
            Expression = expression;
            Name = name;
        }

        public Expression Expression { get; }
        public Identifier Name { get; }
    }
}
