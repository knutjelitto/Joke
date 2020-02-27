using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtViewpointType : PtType
    {
        public PtViewpointType(PonyTokenSpan span, PtType type, PtType arrow)
            : base(span)
        {
            Type = type;
            Arrow = arrow;
        }

        public PtType Type { get; }
        public PtType Arrow { get; }
    }
}
