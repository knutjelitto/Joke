using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Tree
{
    public abstract class LambdaCapture : Node
    {
        protected LambdaCapture(TSpan span)
            : base(span)
        {
        }
    }
}
