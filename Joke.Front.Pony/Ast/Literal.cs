using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Literal : Expression
    {
        public Literal(TSpan span) : base(span)
        {
        }

        public override string? ToString()
        {
            return Span.ToString();
        }
    }
}
