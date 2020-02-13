using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Body : Expression
    {
        public Body(TSpan span, Expression expression)
            : base(span)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
