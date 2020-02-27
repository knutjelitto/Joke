using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtRepeat : PtExpression
    {
        public PtRepeat(PonyTokenSpan span, PtAnnotations? annotations, PtExpression body, PtExpression condition, PtElse? elsePart)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
            Condition = condition;
            ElsePart = elsePart;
        }

        public PtAnnotations? Annotations { get; }
        public PtExpression Body { get; }
        public PtExpression Condition { get; }
        public PtElse? ElsePart { get; }
    }
}
