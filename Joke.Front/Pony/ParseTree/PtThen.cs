using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtThen : PtExpression
    {
        public PtThen(PonyTokenSpan span, PtAnnotations? annotations, PtExpression body)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
        }

        public PtAnnotations? Annotations { get; }
        public PtExpression Body { get; }
    }
}
