using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Call : Expression
    {
        public Call(TSpan span, Expression left, Arguments arguments, Partial? partial)
            : base(span)
        {
            Left = left;
            Arguments = arguments;
            Partial = partial;
        }

        public Expression Left { get; }
        public Arguments Arguments { get; }
        public Partial? Partial { get; }
    }
}
