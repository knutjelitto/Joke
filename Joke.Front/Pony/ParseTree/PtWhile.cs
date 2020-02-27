using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtWhile : PtExpression
    {
        public PtWhile(PonyTokenSpan span, PtAnnotations? annotations, PtExpression condition, PtExpression body, PtExpression? elsePart)
            : base(span)
        {
            Annotations = annotations;
            Condition = condition;
            Body = body;
            ElsePart = elsePart;
        }

        public PtAnnotations? Annotations { get; }
        public PtExpression Condition { get; }
        public PtExpression Body { get; }
        public PtExpression? ElsePart { get; }
    }
}
