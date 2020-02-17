using Joke.Front.Pony.Lex;
using System;
using System.Collections.Generic;
using System.Text;

namespace Joke.Front.Pony.Ast
{
    public class LambdaCaptureThis : LambdaCapture
    {
        public LambdaCaptureThis(TSpan span, ThisLiteral thisLiteral)
            : base(span)
        {
            ThisLiteral = thisLiteral;
        }

        public ThisLiteral ThisLiteral { get; }
    }
}
