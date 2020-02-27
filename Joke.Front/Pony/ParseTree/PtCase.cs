using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtCase : PtNode
    {
        public PtCase(PonyTokenSpan span, PtAnnotations? annotations, PtExpression? pattern, PtExpression? guard, PtExpression? body)
            : base(span)
        {
            Annotations = annotations;
            Pattern = pattern;
            Guard = guard;
            Body = body;
        }

        public PtAnnotations? Annotations { get; }
        public PtExpression? Pattern { get; }
        public PtExpression? Guard { get; }
        public PtExpression? Body { get; }
    }
}
