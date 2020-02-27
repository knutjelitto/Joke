using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtTry : PtExpression
    {
        public PtTry(PonyTokenSpan span, PtAnnotations? annotations, PtExpression body, PtExpression? elsePart, PtExpression? thenPart)
            : base(span)
        {
            Annotations = annotations;
            Body = body;
            ElsePart = elsePart;
            ThenPart = thenPart;
        }

        public PtAnnotations? Annotations { get; }
        public PtExpression Body { get; }
        public PtExpression? ElsePart { get; }
        public PtExpression? ThenPart { get; }
    }
}
