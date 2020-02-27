using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtRef : PtExpression
    {
        public PtRef(PonyTokenSpan span, PtIdentifier name)
            : base(span)
        {
            Name = name;
        }

        public PtIdentifier Name { get; }
    }
}
