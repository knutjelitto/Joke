using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Assignment : Expression
    {
        public Assignment(TSpan span, Expression left, Expression right)
            : base(span)
        {
            Left = left;
            Right = right;
        }

        public Expression Left { get; }
        public Expression Right { get; }
    }
}
