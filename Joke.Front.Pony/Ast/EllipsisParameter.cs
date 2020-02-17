using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class EllipsisParameter : Parameter
    {
        public EllipsisParameter(TSpan span)
            : base(span)
        {
        }
    }
}
