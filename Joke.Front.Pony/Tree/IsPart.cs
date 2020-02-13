using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class IsPart : InfixPart
    {
        public IsPart(TSpan span, Boolean isnt, Expression expression)
            : base(span)
        {
            Isnt = isnt;
            Expression = expression;
        }

        public Boolean Isnt { get; }
        public Expression Expression { get; }
    }
}
