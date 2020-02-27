using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtWith : PtExpression
    {
        public PtWith(PonyTokenSpan span, PtAnnotations? annotations, PtWithElements elements, PtExpression body, PtElse? elsePart)
            : base(span)
        {
            Annotations = annotations;
            Elements = elements;
            Body = body;
            ElsePart = elsePart;
        }

        public PtAnnotations? Annotations { get; }
        public PtWithElements Elements { get; }
        public PtExpression Body { get; }
        public PtElse? ElsePart { get; }
    }
}
