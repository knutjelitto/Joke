using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class ThisType : Type
    {
        public ThisType(TSpan span)
            : base(span)
        {
        }
    }
}
