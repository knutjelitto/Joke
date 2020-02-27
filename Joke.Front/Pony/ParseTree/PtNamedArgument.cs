using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtNamedArgument : PtArgument
    {
        public PtNamedArgument(PonyTokenSpan span, PtIdentifier name, PtExpression value)
            : base(span)
        {
            Name = name;
            Value = value;
        }

        public PtIdentifier Name { get; }
        public PtExpression Value { get; }
    }
}
