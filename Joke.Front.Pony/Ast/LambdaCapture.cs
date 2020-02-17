using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class LambdaCapture : Node
    {
        protected LambdaCapture(TSpan span)
            : base(span)
        {
        }
    }
}
