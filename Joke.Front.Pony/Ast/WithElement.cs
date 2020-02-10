using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class WithElement : Expression
    {
        public WithElement(ISpan span, IdSeq names, Expression initializer)
            : base(span)
        {
        }
    }
}
