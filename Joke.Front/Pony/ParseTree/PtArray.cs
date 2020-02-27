using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtArray : PtExpression
    {
        public PtArray(PonyTokenSpan span, PtType? type, PtExpression? elements)
            : base(span)
        {
            Type = type;
            Elements = elements;
        }

        public PtType? Type { get; }
        public PtExpression? Elements { get; }
    }
}
