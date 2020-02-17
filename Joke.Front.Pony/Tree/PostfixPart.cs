using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public class PostfixPart : Expression
    {
        public PostfixPart(TSpan span) : base(span)
        {
        }
    }
}
