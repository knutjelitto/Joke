using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class IdsSingle : Ids
    {
        public IdsSingle(TSpan span, Identifier name)
            : base(span)
        {
        }
    }
}
