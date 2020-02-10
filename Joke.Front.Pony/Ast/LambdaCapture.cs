using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public abstract class LambdaCapture : Base
    {
        public LambdaCapture(ISpan span) : base(span)
        {
        }
    }
}
