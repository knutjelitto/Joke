using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtIff : PtExpression
    {
        public PtIff(PonyTokenSpan span, PtIffKind kind, PtAnnotations? annotations, PtExpression condition, PtExpression thenPart, PtExpression? elsePart)
            : base(span)
        {
            Kind = kind;
            Annotations = annotations;
            Condition = condition;
            ThenPart = thenPart;
            ElsePart = elsePart;
        }

        public PtIffKind Kind { get; }
        public PtAnnotations? Annotations { get; }
        public PtExpression Condition { get; }
        public PtExpression ThenPart { get; }
        public PtExpression? ElsePart { get; }
    }
}
