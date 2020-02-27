using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtFor : PtExpression
    {
        public PtFor(PonyTokenSpan span, PtAnnotations? annotations, PtIds names, PtExpression iterator, PtExpression body, PtExpression? elsePart)
            : base(span)
        {
            Annotations = annotations;
            Names = names;
            Iterator = iterator;
            Body = body;
            ElsePart = elsePart;
        }

        public PtAnnotations? Annotations { get; }
        public PtIds Names { get; }
        public PtExpression Iterator { get; }
        public PtExpression Body { get; }
        public PtExpression? ElsePart { get; }
    }
}
