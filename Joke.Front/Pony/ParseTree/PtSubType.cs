using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtSubType : PtExpression
    {
        public PtSubType(PonyTokenSpan span, PtType sub, PtType super)
            : base(span)
        {
            Sub = sub;
            Super = super;
        }

        public PtType Sub { get; }
        public PtType Super { get; }
    }
}
