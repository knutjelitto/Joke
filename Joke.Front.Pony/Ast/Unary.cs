using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Unary : Expression
    {
        public Unary(TSpan span, UnaryKind kind, Expression expression)
            : base(span)
        {
            Kind = kind;
            Expression = expression;
        }

        public UnaryKind Kind { get; }
        public Expression Expression { get; }
    }
}
