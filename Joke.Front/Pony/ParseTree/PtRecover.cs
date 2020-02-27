using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtRecover : PtExpression
    {
        public PtRecover(PonyTokenSpan span, PtAnnotations? annotations, PtCap? cap, PtExpression body) : base(span)
        {
            Annotations = annotations;
            Cap = cap;
            Body = body;
        }

        public PtAnnotations? Annotations { get; }
        public PtCap? Cap { get; }
        public PtExpression Body { get; }
    }
}
