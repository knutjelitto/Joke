using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtObject : PtExpression
    {
        public PtObject(PonyTokenSpan span, PtAnnotations? annotations, PtCap? cap, PtType? provides, PtMembers members)
            : base(span)
        {
            Annotations = annotations;
            Cap = cap;
            Provides = provides;
            Members = members;
        }

        public PtAnnotations? Annotations { get; }
        public PtCap? Cap { get; }
        public PtType? Provides { get; }
        public PtMembers Members { get; }
    }
}
