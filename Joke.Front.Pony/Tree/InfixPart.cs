using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class InfixPart : Node
    {
        public InfixPart(TSpan span)
            : base(span)
        {
        }
    }
}
