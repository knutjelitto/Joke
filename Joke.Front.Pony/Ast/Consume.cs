using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Consume : Expression
    {
        public Consume(TSpan span, Cap? cap, Expression expression)
            : base(span)
        {
            Cap = cap;
            Expression = expression;
        }

        public Cap? Cap { get; }
        public Expression Expression { get; }
    }
}
