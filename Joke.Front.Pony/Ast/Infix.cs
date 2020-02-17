using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class Infix : Expression
    {
        public Infix(TSpan span, Expression left, IReadOnlyList<InfixPart> parts)
            : base(span)
        {
            Left = left;
            Parts = parts;
        }

        public Expression Left { get; }
        public IReadOnlyList<InfixPart> Parts { get; }
    }
}
