using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Call : PostfixPart
    {
        public Call(TSpan span, Arguments arguments, Partial? partial)
            : base(span)
        {
            Arguments = arguments;
            Partial = partial;
        }

        public Arguments Arguments { get; }
        public Partial? Partial { get; }
    }
}
