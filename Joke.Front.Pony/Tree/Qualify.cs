using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Qualify : Expression
    {
        public Qualify(TSpan span, Expression left, TypeArguments arguments)
            : base(span)
        {
            Left = left;
            Arguments = arguments;
        }

        public Expression Left { get; }
        public TypeArguments Arguments { get; }
    }
}
