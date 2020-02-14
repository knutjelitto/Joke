using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class Call : Expression
    {
        public Call(TSpan span, Arguments arguments)
            : base(span)
        {
            Arguments = arguments;
        }

        public Arguments Arguments { get; }
    }
}
