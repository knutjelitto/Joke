using Joke.Front.Pony.Lexing;

namespace Joke.Front.Pony.ParseTree
{
    public class PtCall : PtPostfixPart
    {
        public PtCall(PonyTokenSpan span, PtArguments arguments, bool partial)
            : base(span)
        {
            Arguments = arguments;
            Partial = partial;
        }

        public PtArguments Arguments { get; }
        public bool Partial { get; }
    }
}
