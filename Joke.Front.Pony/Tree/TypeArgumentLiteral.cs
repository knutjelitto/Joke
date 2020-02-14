using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class TypeArgumentLiteral : TypeArgument
    {
        public TypeArgumentLiteral(TSpan span, Expression literal)
            : base(span)
        {
            Literal = literal;
        }

        public Expression Literal { get; }
    }
}
