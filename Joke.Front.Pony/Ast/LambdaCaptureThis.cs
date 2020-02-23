using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.Ast
{
    public class LambdaCaptureThis : LambdaCapture
    {
        public LambdaCaptureThis(TokenSpan span, ThisLiteral thisLiteral)
            : base(span)
        {
            ThisLiteral = thisLiteral;
        }

        public ThisLiteral ThisLiteral { get; }
    }
}
