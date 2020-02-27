using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtArrayType : PtType
    {
        public PtArrayType(PonyTokenSpan span, PtType type)
            : base(span)
        {
            Type = type;
        }

        public PtType Type { get; }
    }
}
