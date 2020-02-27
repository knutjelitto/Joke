using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtPositionalArgument : PtArgument
    {
        public PtPositionalArgument(PonyTokenSpan span, PtExpression value)
            : base(span)
        {
            Value = value;
        }

        public PtExpression Value { get; }
    }
}
