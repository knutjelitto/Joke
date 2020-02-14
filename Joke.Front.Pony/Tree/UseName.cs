using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class UseName : Identifier
    {
        public UseName(TSpan span, Identifier name) : base(span)
        {
            Name = name;
        }

        public Identifier Name { get; }
    }
}
