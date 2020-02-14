using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Sequence : Expression
    {
        public Sequence(TSpan span, Expression first, Expression next)
            : base(span)
        {
            First = first;
            Next = next;
        }

        public Expression First { get; }
        public Expression Next { get; }
    }
}
