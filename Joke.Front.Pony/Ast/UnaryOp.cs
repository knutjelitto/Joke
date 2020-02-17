using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class UnaryOp : Expression
    {
        public UnaryOp(TSpan span, UnaryOpKind kind, Expression expression)
            : base(span)
        {
            Kind = kind;
            Expression = expression;
        }

        public UnaryOpKind Kind { get; }
        public Expression Expression { get; }
    }
}
