using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtMatch : PtExpression
    {
        public PtMatch(PonyTokenSpan span, PtAnnotations? annotations, PtExpression toMatch, PtCases cases, PtExpression? elsePart)
            : base(span)
        {
            Annotations = annotations;
            ToMatch = toMatch;
            Cases = cases;
            ElsePart = elsePart;
        }

        public PtAnnotations? Annotations { get; }
        public PtExpression ToMatch { get; }
        public PtCases Cases { get; }
        public PtExpression? ElsePart { get; }
    }
}
