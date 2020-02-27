using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtAs : PtExpression
    {
        public PtAs(PonyTokenSpan span, PtExpression value, PtType type)
            : base(span)
        {
            Value = value;
            Type = type;
        }

        public PtExpression Value { get; }
        public PtType Type { get; }
    }
}
