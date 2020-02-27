using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtLambdaCaptureThis : PtLambdaCapture
    {
        public PtLambdaCaptureThis(PonyTokenSpan span, PtThisLiteral thisLiteral)
            : base(span)
        {
            ThisLiteral = thisLiteral;
        }

        public PtThisLiteral ThisLiteral { get; }
    }
}
