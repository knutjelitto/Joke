using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class LambdaCaptureThis : LambdaCapture
    {
        public LambdaCaptureThis(ISpan span)
            : base(span)
        {
        }
    }
}
