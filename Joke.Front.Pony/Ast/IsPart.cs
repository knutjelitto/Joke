using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class IsPart : InfixPart
    {
        public IsPart(TSpan span, BinaryKind kind, Expression expression)
            : base(span, kind)
        {
            Expression = expression;
        }

        public Expression Expression { get; }
    }
}
